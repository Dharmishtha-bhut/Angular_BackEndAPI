using Backend.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace Backend.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        // ✅ Backing field (IMPORTANT)
        private Expression<Func<T, bool>>? _criteria;

        protected BaseSpecification(): this(null)
        {
        }

        public BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            _criteria = criteria;
        }

        // ✅ FIXED (no recursion)
        public Expression<Func<T, bool>>? Criteria => _criteria;

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        public bool IsDistinct { get; private set; }

        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            _criteria = criteria;
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyDistinct()
        {
            IsDistinct = true;
        }
    }
    public class BaseSpecification<T, TResult>(Expression<Func<T,bool>> criteria) 
        : BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
        protected BaseSpecification() : this(null!) { }
        
        public Expression<Func<T, TResult>>? Select { get; private set; }
        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            Select = selectExpression;
        }
    }
}