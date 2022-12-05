using ArtGallery.Core.Contracts;
using ArtGallery.Core.Models;
using ArtGallery.Infrastructure.Data.Models;
using ArtGallery.Infrastructure.Data;
using Grpc.Core;
using System.Linq.Expressions;
using ArtGallery.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Core.Common;
using ArtGallery.Core.Constants;

namespace ArtGallery.Core.Services
{ 
    public class StoreService : DataService, IStoreService
    {
        public StoreService(IApplicationDbRepository repo)
             : base(repo)
        {
        }
        public async Task<StorePageViewModel> GetStorePageAsync(int page, int size, string category = "")
        {
            return await this.GetStorePageAsync(page, size, p => p.Price, category);
        }

        public async Task<StorePageViewModel> GetStorePageAsync(int page, int size, Expression<Func<Painting, object>> orderByExpression, string category = "")
        {
            var noFilter = string.IsNullOrWhiteSpace(category);
            var pageCount = await this.GetPaintingPagesCountAsync(size, p => p.Category.Name == category || noFilter);
            if (page >= pageCount) page = 0;

            Guard.AgainstNull(orderByExpression, nameof(orderByExpression));

            var paintings = await this.repo.All<Painting>()
                .Where(p => p.Category.Name == category || noFilter)
                .OrderBy(orderByExpression)
                .Skip(page * size)
                .Take(size)
                .Select(p => new PaintingViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.ImagePath
                }).ToListAsync();

            if (pageCount == 0) pageCount = 1;

            return new StorePageViewModel
            {
                PageNumber = page + 1,
                PagesCount = pageCount,
                Paintings = paintings
            };
        }

        public async Task<int> GetPaintingPagesCountAsync(int size, Expression<Func<Painting, bool>> filterExpression)
        {
            Guard.AgainstNull(filterExpression, nameof(filterExpression));
            if (size == 0) return 0;

            return (int)Math.Ceiling(await this.repo.All<Painting>()
                .Where(filterExpression)
                .CountAsync() / (double)size);
        }

        public async Task<PaintingPageViewModel?> GetPaintingPageAsync(string paintingId)
        {
            Guard.AgainstNullOrWhiteSpaceString(paintingId, nameof(paintingId));
            Guid id;
            try
            {
                id = Guid.Parse(paintingId);
            }
            catch (Exception)
            {
                return null;
            }

            var painting = await this.repo.All<Painting>()
                .Where(p => p.Id == id)
                .Select(p => new PaintingPageViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Image = p.ImagePath
                })
                .SingleOrDefaultAsync();

            return painting;
        }

        public async Task<(bool, string)> AddPaintingToCartAsync(string? userId, string? paintingId, int quantity = 1)
        {
            if (quantity <= 0)
            {
                return (false, "Quantity cannot be zero.");
            }
            try
            {
                var user = await this.repo.All<ApplicationUser>()
                    .Where(u => u.Id == userId)
                    .Include(u => u.Cart)
                    .SingleOrDefaultAsync();

                Guard.AgainstNull(user, nameof(user));
                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId = userId };
                }

                var painting = await this.repo.GetByIdAsync<Painting>(Guid.Parse(paintingId));
                Guard.AgainstNull(painting, nameof(painting));

                var cp = new CartPainting
                {
                    Cart = user.Cart,
                    Painting = painting
                };

                await this.repo.AddAsync(cp);
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (false, ExceptionMessageConstants.UnexpectedErrorMessage);
            }

            return (true, string.Empty);
        }

        public async Task<CartViewModel> GetCartAsync(string? userId)
        {
            Guard.AgainstNullOrWhiteSpaceString(userId, nameof(userId));

            var cart = await this.repo.All<Cart>()
                .Where(c => c.UserId == userId)
                .Select(c => new CartViewModel
                {
                    Id = c.Id.ToString(),
                    Paintings = c.CartPainting
                        .Where(cp => cp.Ordered == false)
                        .Select(cp => new CartPaintingViewModel
                        {
                            Id = cp.Painting.Id.ToString(),
                            Name = cp.Painting.Name,
                            Price = cp.Painting.Price,
                            ImagePath = cp.Painting.ImagePath
                        })
                        .ToList()
                }).SingleOrDefaultAsync();

            if (cart != null) return cart;

            if (!await this.repo.All<ApplicationUser>().AnyAsync(u => u.Id == userId)) return null;

            var cartId = Guid.NewGuid();
            var newCart = new Cart
            {
                Id = cartId,
                UserId = userId
            };
            await this.repo.AddAsync(newCart);
            await this.repo.SaveChangesAsync();

            return new CartViewModel() { Id = cartId.ToString(), Paintings = new List<CartPaintingViewModel>() };

        }

        public async Task<(bool, string)> PlaceOrderAsync(PlaceOrderPaintingModel[]? paintings, string userId)
        {
            try
            {
                Guard.AgainstNull(paintings, nameof(paintings));
                Guard.AgainstNullOrWhiteSpaceString(userId, nameof(userId));

                var order = new Order
                {
                    UserId = userId,
                    TimeOfOrdering = DateTime.Now,
                    Address = "",
                    OrderPaintings = paintings
                        .Select(p => new OrderPainting
                        {
                            PaintingId = Guid.Parse(p.Id)
                        }).ToList()
                };
                var cartId = await
                    this.repo.All<Cart>()
                        .Where(c => c.UserId == userId)
                        .Select(c => c.Id)
                        .FirstOrDefaultAsync();

                var cartPaintings = await this.repo.All<CartPainting>()
                    .Where(cp => cp.CartId == cartId
                                 && paintings.Select(p => p.Id).Contains(cp.PaintingId.ToString())
                                 && cp.Ordered == false)
                    .ToListAsync();

                foreach (var painting in cartPaintings)
                {
                    if (painting != null)
                        painting.Ordered = true;
                }

                await this.repo.AddAsync(order);
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (false, ExceptionMessageConstants.UnexpectedErrorMessage);
            }

            return (true, string.Empty);
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrdersByUserAsync(string userId)
        {
            Guard.AgainstNullOrWhiteSpaceString(userId, nameof(userId));

            if (!await this.repo.All<ApplicationUser>().AnyAsync(u => u.Id == userId)) return null;

            return await this.repo.All<Order>()
                .Where(o => o.User.Id == userId)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id.ToString(),
                    UserName = o.User.UserName,
                    OrderTime = o.TimeOfOrdering,
                    Paintings = o.OrderPaintings.Select(op => new OrderPaintingViewModel
                    {
                        Id = op.Painting.Id.ToString(),
                        Name = op.Painting.Name,
                        Price = op.Painting.Price,
                    }).ToList()
                })
                .OrderByDescending(o => o.Status)
                .ThenByDescending(o => o.OrderTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllOrdersByStatusAsync(Status status)
        {
            Guard.AgainstNull(status, nameof(status));
            return await this.repo.All<Order>()
                .Select(o => new OrderViewModel
                {
                    Id = o.Id.ToString(),
                    UserName = o.User.UserName,
                    OrderTime = o.TimeOfOrdering,
                    Paintings = o.OrderPaintings.Select(op => new OrderPaintingViewModel
                    {
                        Id = op.Painting.Id.ToString(),
                        Name = op.Painting.Name,
                        Price = op.Painting.Price,
                    }).ToList()
                })
                .OrderByDescending(o => o.OrderTime)
                .ToListAsync();
        }

        public async Task MarkOrderAsFinisedAsync(string orderId)
        {
            Guard.AgainstNullOrWhiteSpaceString(orderId, nameof(orderId));
            var order = await this.repo.All<Order>()
                .Where(o => o.Id == Guid.Parse(orderId))
                .FirstOrDefaultAsync();
            Guard.AgainstNull(order, nameof(order));

            await this.repo.SaveChangesAsync();
        }

        public async Task<bool> CreateNewPaintingAsync(string name, string imageName, decimal price, string description)
         => await CreateNewPaintingAsync(name, imageName, price, description, "General");

        public async Task<bool> CreateNewPaintingAsync(string name, string imageName, decimal price, string description,
            string categoryName)
        {
            if (price <= 0) return false;
            try
            {
                var category = await this.repo.All<Category>()
                    .FirstOrDefaultAsync(c => c.Name == categoryName) ?? new Category { Name = categoryName };

                var painting = new Painting
                {
                    Name = name,
                    Description = description,
                    ImagePath = imageName,
                    Price = price,
                    Category = category,
                };

                await this.repo.AddAsync(painting);
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemovePaintingAsync(string id)
        {
            try
            {
                Guard.AgainstNullOrWhiteSpaceString(id);
                await this.repo.DeleteAsync<Painting>(Guid.Parse(id));
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveFromCartAsync(string userId, string paintingId)
        {
            try
            {
                Guard.AgainstNullOrWhiteSpaceString(userId, nameof(userId));
                Guard.AgainstNullOrWhiteSpaceString(paintingId, nameof(paintingId));

                var cp = await this.repo.All<CartPainting>()
                    .Where(cp => cp.PaintingId == Guid.Parse(paintingId) && cp.Cart.UserId == userId && cp.Ordered == false)
                    .FirstOrDefaultAsync();
                Guard.AgainstNull(cp, "Painting");

                await this.repo.DeleteAsync<CartPainting>(cp.Id);
                await this.repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}