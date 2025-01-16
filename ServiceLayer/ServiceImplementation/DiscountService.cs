using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataMapper;
using DomainModel.Models;
using log4net;

namespace ServiceLayer.ServiceImplementation
{
    /// <summary>
    /// Provides services for managing discounts.
    /// </summary>
    public class DiscountService : IDiscountService
    {
        /// <summary>
        /// Logger instance for logging.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DiscountService));

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountService"/> class.
        /// </summary>
        public DiscountService()
        {
            Logger.Info("DiscountService initialized.");
        }

        /// <summary>
        /// Adds a new discount.
        /// </summary>
        /// <param name="discount">The discount to add.</param>
        public void AddDiscount(Discount discount)
        {
            Logger.Info("Adding a new discount.");
            this.ValidateDiscount(discount);
            DAOFactoryMethod.CurrentDAOFactory.DiscountDataServices.AddDiscount(discount);
        }

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="discount">The discount to update.</param>
        public void UpdateDiscount(Discount discount)
        {
            Logger.Info("Updating an existing discount.");
            this.ValidateDiscount(discount);
            DAOFactoryMethod.CurrentDAOFactory.DiscountDataServices.UpdateDiscount(discount);
        }

        /// <summary>
        /// Deletes a discount.
        /// </summary>
        /// <param name="discount">The discount to delete.</param>
        public void DeleteDiscount(Discount discount)
        {
            Logger.Info("Deleting a discount.");
            if (discount == null)
            {
                Logger.Error("Discount not found.", new ArgumentException("Discount not found.", nameof(discount)));
                throw new ArgumentException("Discount not found.", nameof(discount));
            }

            DAOFactoryMethod.CurrentDAOFactory.DiscountDataServices.DeleteDiscount(discount);
        }

        /// <summary>
        /// Gets a discount by its ID.
        /// </summary>
        /// <param name="discountId">The ID of the discount.</param>
        /// <returns>The discount with the specified ID.</returns>
        public Discount GetDiscountById(int discountId)
        {
            Logger.Info($"Getting discount by ID: {discountId}.");
            if (discountId <= 0)
            {
                Logger.Error("Discount ID must be greater than zero.", new ArgumentOutOfRangeException(nameof(discountId), "Discount ID must be greater than zero."));
                throw new ArgumentOutOfRangeException(nameof(discountId), "Discount ID must be greater than zero.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.DiscountDataServices.GetDiscountById(discountId);
        }

        /// <summary>
        /// Gets all discounts.
        /// </summary>
        /// <returns>A list of all discounts.</returns>
        public IList<Discount> GetAllDiscounts()
        {
            Logger.Info("Getting all discounts.");
            return DAOFactoryMethod.CurrentDAOFactory.DiscountDataServices.GetAllDiscounts();
        }

        /// <summary>
        /// Validates a discount.
        /// </summary>
        /// <param name="discount">The discount to validate.</param>
        public void ValidateDiscount(Discount discount)
        {
            Logger.Info("Validating a discount.");
            if (discount == null)
            {
                Logger.Error("Discount cannot be null.", new ArgumentNullException(nameof(discount), "Discount cannot be null."));
                throw new ArgumentNullException(nameof(discount), "Discount cannot be null.");
            }

            discount.Validate();
        }
    }
}