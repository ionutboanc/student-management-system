namespace DataMapper.SqlServerDAO
{
    using System.Collections.Generic;
    using System.Linq;
    using DomainModel.Models;

    /// <summary>
    /// Provides SQL Server data services for managing discounts.
    /// </summary>
    public class SQLDiscountDataServices : IDiscountDataServices
    {
        /// <summary>
        /// Adds a new discount.
        /// </summary>
        /// <param name="discount">The discount to add.</param>
        public void AddDiscount(Discount discount)
        {
            using (var context = new MyApplicationContext())
            {
                context.Discounts.Add(discount);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a discount.
        /// </summary>
        /// <param name="discount">The discount to delete.</param>
        public void DeleteDiscount(Discount discount)
        {
            using (var context = new MyApplicationContext())
            {
                var discountToDelete = new Discount { Id = discount.Id };
                context.Discounts.Attach(discountToDelete);
                context.Discounts.Remove(discountToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all discounts.
        /// </summary>
        /// <returns>A list of all discounts.</returns>
        public IList<Discount> GetAllDiscounts()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Discounts.ToList();
            }
        }

        /// <summary>
        /// Gets a discount by its ID.
        /// </summary>
        /// <param name="discountId">The ID of the discount.</param>
        /// <returns>The discount with the specified ID.</returns>
        public Discount GetDiscountById(int discountId)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Discounts.SingleOrDefault(d => d.Id == discountId);
            }
        }

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="discount">The discount to update.</param>
        public void UpdateDiscount(Discount discount)
        {
            using (var context = new MyApplicationContext())
            {
                context.Discounts.Attach(discount);
                context.Entry(discount).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}