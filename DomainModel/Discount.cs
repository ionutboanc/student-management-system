namespace DomainModel.Models
{
    using System.Collections.Generic;

    // Model for Discount
    public class Discount
    {
        public int Id { get; set; }
        public decimal DiscountPercentage { get; set; }
        public virtual List<Course> Courses { get; set; }
        public Discount()
        {
            this.Courses = new List<Course>();
        }
        public Discount(decimal discountPercentage)
        {
            this.Courses = new List<Course>();
            this.DiscountPercentage = discountPercentage;
        }

        public Discount(List<Course> courses, decimal discountPercentage)
        {
            this.Courses = courses;
            this.DiscountPercentage = discountPercentage;
        }
    }
}