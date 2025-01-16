// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System;
    using System.Configuration;
    using DataMapper.SqlServerDAO;

    /// <summary>
    /// Provides a method to get the current DAO factory.
    /// </summary>
    public static class DAOFactoryMethod
    {
        /// <summary>
        /// The current DAO factory instance.
        /// </summary>
        private static readonly IDAOFactory CurrentDaoFactory;

        /// <summary>
        /// Initializes static members of the <see cref="DAOFactoryMethod"/> class.
        /// </summary>
        static DAOFactoryMethod()
        {
            string currentDataProvider = ConfigurationManager.AppSettings["dataProvider"];
            if (string.IsNullOrWhiteSpace(currentDataProvider))
            {
                CurrentDaoFactory = null;
            }
            else
            {
                switch (currentDataProvider.ToLower().Trim())
                {
                    case "sqlserver":
                        CurrentDaoFactory = new SQLServerDAOFactory();
                        break;
                    default:
                        CurrentDaoFactory = new SQLServerDAOFactory();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the current DAO factory.
        /// </summary>
        /// <value>The current DAO factory.</value>
        public static IDAOFactory CurrentDAOFactory
        {
            get
            {
                return CurrentDaoFactory;
            }
        }
    }
}