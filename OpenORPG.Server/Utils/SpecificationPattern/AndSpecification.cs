using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utils.SpecificationPattern
{
    internal class AndSpecification<TEntity> : ISpecification<TEntity>
    {
        private ISpecification<TEntity> _specificationOne;
        private ISpecification<TEntity> _specificationTwo;

        internal AndSpecification(ISpecification<TEntity> specificationOne,
          ISpecification<TEntity> specificationTwo)
        {
            this._specificationOne = specificationOne;
            this._specificationTwo = specificationTwo;
        }

        public bool IsSatisfiedBy(TEntity entity)
        {
            return (this._specificationOne.IsSatisfiedBy(entity) &&
              this._specificationTwo.IsSatisfiedBy(entity));
        }
    }  

}
