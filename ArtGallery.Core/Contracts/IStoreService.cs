using ArtGallery.Core.Models;
using ArtGallery.Infrastructure.Data.Models;
using Grpc.Core;
using System.Linq.Expressions;

namespace ArtGallery.Core.Contracts
{
    public interface IStoreService
    {
        /// <summary>
        /// Used to get a store page with its paintings.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="size">The number of paintings to be displayed in a page.</param>
        /// <param name="category">Filters paintings by category.</param>
        /// <returns>Returns a store page view model.</returns>
        public Task<StorePageViewModel> GetStorePageAsync(int page, int size, string category = "");

        public Task<StorePageViewModel> GetStorePageAsync(int page, int size,
            Expression<Func<Painting, object>> orderByExpression, string category = "");
        /// <summary>
        /// Used to get the number of all pages.
        /// </summary>
        /// <param name="size">The size of the page</param>
        /// <param name="filterExpression">An expression used to filter the paintings.</param>
        /// <returns>Returns the number of all pages.</returns>
        public Task<int> GetPaintingPagesCountAsync(int size, Expression<Func<Painting, bool>> filterExpression);
        /// <summary>
        /// Used to get a given painting model.
        /// </summary>
        /// <param name="paintingId">The painting id.</param>
        /// <returns>Returns a painting page view model.</returns>
        public Task<PaintingPageViewModel?> GetPaintingPageAsync(string paintingId);
        /// <summary>
        /// Used to add a painting to a user's cart.
        /// </summary>
        /// <param name="userId">The user who owns the cart.</param>
        /// <param name="paintingId">The painting to be added.</param>
        /// <param name="quantity">The quantity of the painting to be added.</param>
        /// <returns>Returns true if adding was successful. Returns false and a string with an error message if it was unsuccessful.</returns>
        public Task<(bool, string)> AddPaintingToCartAsync(string userId, string paintingId, int quantity = 1);
        /// <summary>
        /// Used to get a user's cart.
        /// </summary>
        /// <param name="userId">The user who owns the cart.</param>
        /// <returns>Returns a cart view model with painting.</returns>
        public Task<CartViewModel> GetCartAsync(string userId);
        /// <summary>
        /// Used to create a new order.
        /// </summary>
        /// <returns>Returns true if adding was successful. Returns false and a string with an error message if it was unsuccessful.</returns>
        public Task<(bool, string)> PlaceOrderAsync(PlaceOrderPaintingModel[]? paintings, string userId);
        /// <summary>
        /// Used to get all of a given user's orders.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns a collection of order view models.</returns>
        public Task<IEnumerable<OrderViewModel>> GetOrdersByUserAsync(string userId);
        /// <summary>
        /// Used to get all orders filtered by their status.
        /// </summary>
        /// <param name="status"></param>
        /// <returns>Returns a collection of order view models.</returns>
        public Task<IEnumerable<OrderViewModel>> GetAllOrdersByStatusAsync(Status status);

        public Task MarkOrderAsFinisedAsync(string orderId);
        Task<bool> CreateNewPaintingAsync(string name, string imageName, decimal price, string description);
        Task<bool> CreateNewPaintingAsync(string name, string imageName, decimal price, string description, string categoryName);
        Task<bool> RemovePaintingAsync(string id);
        Task<bool> RemoveFromCartAsync(string userId, string paintingId);
    }
}