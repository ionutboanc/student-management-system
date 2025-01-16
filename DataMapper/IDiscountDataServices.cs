// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System.Collections.Generic;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for discount data services.
    /// </summary>
    public interface IDiscountDataServices
    {
        /// <summary>
        /// Adds a new discount.
        /// </summary>
        /// <param name="discount">The discount to add.</param>
        void AddDiscount(Discount discount);

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="discount">The discount to update.</param>
        void UpdateDiscount(Discount discount);

        /// <summary>
        /// Deletes a discount.
        /// </summary>
        /// <param name="discount">The discount to delete.</param>
        void DeleteDiscount(Discount discount);

        /// <summary>
        /// Gets a discount by its ID.
        /// </summary>
        /// <param name="discountId">The ID of the discount.</param>
        /// <returns>The discount with the specified ID.</returns>
        Discount GetDiscountById(int discountId);

        /// <summary>
        /// Gets all discounts.
        /// </summary>
        /// <returns>A list of all discounts.</returns>
        IList<Discount> GetAllDiscounts();
    }
}