using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils.SpecificationPattern
{
    public static class SpecificationExtensionMethods
    {
        public static ISpecification<TEntity> And<TEntity>(
          this ISpecification<TEntity> specificationOne,
          ISpecification<TEntity> specificationTwo)
        {
            return new AndSpecification<TEntity>(
              specificationOne, specificationTwo);
        }


        // We can can add more if we need, applying YAGNI

    }


}
