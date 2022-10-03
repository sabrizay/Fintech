using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Util
{
    public abstract class Token { }
    public class MethodCallToken : Token
    {
        public string MethodName { get; set; }

        public MethodCallToken(string methodName)
        {
            MethodName = methodName;
        }

        public override string ToString()
        {
            return "Method call token:\t" + MethodName;
        }
    }
    public class UnaryOperatorToken : Token
    {
        public FilterOperator Operator { get; set; }

        public UnaryOperatorToken(FilterOperator op)
        {
            Operator = op;
        }

        public override string ToString()
        {
            return "Unary operator token:\t\t" + Operator.ToString();
        }
    }

    public class MemberToken : Token
    {
        public Type Type { get; set; }

        public string MemberName { get; set; }
        public object MemberValue { get; set; }

        public MemberToken(string memberName, Type type, object memberValue = null)
        {
            MemberName = memberName;
            Type = type;
            MemberValue = memberValue;
        }

        public override string ToString()
        {
            return "Member token:\t\t" + MemberName;
        }
    }
    public class ConstantToken : Token
    {
        public object Value { get; set; }

        public ConstantToken(object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return "Constant token:\t\t" + Value.ToString();
        }
    }
    public class BinaryOperatorToken : Token
    {
        public FilterOperator Operator { get; set; }

        public BinaryOperatorToken(FilterOperator op)
        {
            Operator = op;
        }

        public override string ToString()
        {
            return "Binary operator token:\t" + Operator.ToString();
        }
    }
    public class ParameterToken : Token
    {
        public string ParameterName { get; set; }
        public Type Type { get; set; }

        public ParameterToken(string name, Type type)
        {
            ParameterName = name;
            Type = type;
        }

        public override string ToString()
        {
            return "Parameter token:\t\t" + ParameterName;
        }
    }

    public enum FilterOperator
    {
        NOT_SET,

        // Logical
        And,
        Or,
        Not,

        // Comparison
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,

        // String
        StartsWith,
        Contains,
        EndsWith,
        NotStartsWith,
        NotContains,
        NotEndsWith

    }

    public class DapperExpression : ExpressionVisitor
    {
        private readonly List<Token> _tokens = new();

        static string OperatorMapToString(FilterOperator ft) => (_operatorMapString.ContainsKey(ft) ? _operatorMapString[ft] : "");

        public List<Token> Tokens => _tokens;
        private readonly Expression fixedExpression;
        public DapperExpression(Expression expression)
        {
            var fixer = new BooleanVisitor();
            fixedExpression = fixer.Visit(expression);
            base.Visit(fixedExpression);
        }

        private static readonly Dictionary<ExpressionType, FilterOperator> _operatorMap = new()
        {
            { ExpressionType.AndAlso           , FilterOperator.And                },
            { ExpressionType.OrElse            , FilterOperator.Or                 },
            { ExpressionType.Not               , FilterOperator.Not                },
            { ExpressionType.Equal             , FilterOperator.Equal              },
            { ExpressionType.NotEqual          , FilterOperator.NotEqual           },
            { ExpressionType.LessThan          , FilterOperator.LessThan           },
            { ExpressionType.LessThanOrEqual   , FilterOperator.LessThanOrEqual    },
            { ExpressionType.GreaterThan       , FilterOperator.GreaterThan        },
            { ExpressionType.GreaterThanOrEqual, FilterOperator.GreaterThanOrEqual },

        };
        private static readonly Dictionary<FilterOperator, string> _operatorMapString = new()
        {
            { FilterOperator.And           , " and "               },
            { FilterOperator.Or            , " or "                 },
            { FilterOperator.Not               , " not "               },
            { FilterOperator.Equal             , " = "              },
            { FilterOperator.NotEqual          , " <> "           },
            { FilterOperator.LessThan          , " < "           },
            { FilterOperator.LessThanOrEqual   , " <= "    },
            { FilterOperator.GreaterThan       , " > "        },
            { FilterOperator.GreaterThanOrEqual, " >= " },

        };
        private static readonly Dictionary<string, FilterOperator> _methodCallMap = new()
        {
            { nameof(FilterOperator.StartsWith)   , FilterOperator.StartsWith },
            { nameof(FilterOperator.Contains)     , FilterOperator.Contains   },
            { nameof(FilterOperator.EndsWith)     , FilterOperator.EndsWith   },
            { nameof(FilterOperator.NotStartsWith), FilterOperator.NotStartsWith },
            { nameof(FilterOperator.NotContains)  , FilterOperator.NotContains   },
            { nameof(FilterOperator.NotEndsWith)  , FilterOperator.NotEndsWith   }
        };
        public IEnumerable<FilterDescriptor> Build()
        {
            var filters = new Stack<FilterDescriptor>();

            for (var i = 0; i < _tokens.Count; i++)
            {
                var token = _tokens[i];

                switch (token)
                {

                    case ParameterToken p:
                        var f = getFilter();
                        f.FieldName = p.ParameterName;
                        f.StringOperator = "";
                        filters.Push(f);
                        break;

                    case BinaryOperatorToken b:
                        var f1 = getFilter();
                        f1.StringOperator = OperatorMapToString(b.Operator);
                        switch (b.Operator)
                        {
                            case FilterOperator.And:
                            case FilterOperator.Or:
                                if (filters.Any())
                                {
                                    var ff = filters.Pop();
                                    ff.CompositionOperator = b.Operator;
                                    filters.Push(ff);
                                }
                                break;

                            case FilterOperator.Equal:
                            case FilterOperator.NotEqual:
                            case FilterOperator.LessThan:
                            case FilterOperator.LessThanOrEqual:
                            case FilterOperator.GreaterThan:
                            case FilterOperator.GreaterThanOrEqual:
                                f1.Operator = b.Operator;

                                filters.Push(f1);
                                break;
                        }

                        break;

                    case ConstantToken c:
                        var f2 = getFilter();

                        f2.Value = c.Value;
                        filters.Push(f2);
                        break;

                    case MemberToken m:
                        var f3 = getFilter();
                        f3.FieldName ??= m.MemberName;
                        f3.Value = m.MemberValue;
                        filters.Push(f3);
                        break;

                    case UnaryOperatorToken u:
                        var f4 = getFilter();
                        f4.Operator = u.Operator;
                        f4.StringOperator = OperatorMapToString(u.Operator);
                        f4.Value = true;
                        filters.Push(f4);
                        break;

                    case MethodCallToken mc:
                        var f5 = getFilter();
                        f5.Operator = _methodCallMap[mc.MethodName];
                        f5.StringOperator = OperatorMapToString(f5.Operator);

                        //f5.Value = mc.Args[0];
                        filters.Push(f5);
                        break;
                }
            }

            var output = new Stack<FilterDescriptor>();

            while (filters.Any())
            {
                output.Push(filters.Pop());
            }

            return output;

            FilterDescriptor getFilter()
            {
                if (filters.Any())
                {
                    var f = filters.First();


                    var incomplete = f.Operator == default ||
                                     f.CompositionOperator == default ||
                                     f.FieldName == default ||
                                     f.StringOperator == default
                                     || f.Value == default;



                    f.InComplete = incomplete;
                    if (incomplete)
                        return filters.Pop();

                    return new FilterDescriptor();
                }

                return new FilterDescriptor();
            }
        }
        #region Overrides

        protected override Expression VisitBinary(BinaryExpression node)
        {
            base.Visit(node.Left);
            base.Visit(node.Right);

            switch (node.NodeType)
            {
                case ExpressionType.OrElse:
                case ExpressionType.AndAlso:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                    _tokens.Add(new BinaryOperatorToken(_operatorMap[node.NodeType]));
                    break;
            }

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _tokens.Add(new ConstantToken(node.Value));
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            //if (node.Type == typeof(DateTime))
            //{
            //    if (node.Expression == null)
            //    {
            //        var lambda = Expression.Lambda<Func<DateTime>>(node);
            //        var dateTime = lambda.Compile()();
            //        base.Visit(Expression.Constant(dateTime.Ticks));
            //        return node;
            //    }
            //    else
            //    {
            //        switch (node.Expression.NodeType)
            //        {
            //            case ExpressionType.New:
            //                var lambda = Expression.Lambda<Func<DateTime>>(node.Expression);
            //                var dateTime = lambda.Compile()();
            //                base.Visit(Expression.Constant(dateTime.Ticks));
            //                return node;

            //            case ExpressionType.MemberAccess:
            //                if (node.Member.Name != ((MemberExpression)node.Expression).Member.Name)
            //                {
            //                    var lambda2 = Expression.Lambda<Func<DateTime>>(node);
            //                    var dateTime2 = lambda2.Compile()();
            //                    base.Visit(Expression.Constant(dateTime2.Ticks));
            //                    return node;
            //                }
            //                break;
            //        }
            //    }
            //}

            _tokens.Add(new MemberToken(node.Expression.Type.Name + "." + node.Member.Name, node.Member.DeclaringType, GetMemberValue(node)));
            return node;
        }
        private static readonly string[] _supprtedMethodNames = new[]
{
            nameof(string.StartsWith),
            nameof(string.Contains),
            nameof(string.EndsWith)
        };
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (!_supprtedMethodNames.Contains(node.Method.Name))
                throw new NotSupportedException($"{node.Method.Name} method is not supported.");

            base.Visit(node.Object);
            base.Visit(node.Arguments);
            _tokens.Add(new MethodCallToken(node.Method.Name));
            return node;
        }
        private static object GetMemberValue(MemberExpression member)
        {

            dynamic getter;
            try
            {
                var objectMember = Expression.Convert(member, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);


                getter = getterLambda.Compile();
            }
            catch (Exception) { return null; }

            return getter();
        }
        protected override Expression VisitUnary(UnaryExpression node)
        {
            base.Visit(node.Operand);

            if (node.NodeType == ExpressionType.Not)
            {
                var token = _tokens.Last();

                if (token is BinaryOperatorToken binaryToken)
                {
                    switch (binaryToken.Operator)
                    {
                        case FilterOperator.Equal:
                            binaryToken.Operator = FilterOperator.NotEqual;
                            break;

                        case FilterOperator.NotEqual:
                            binaryToken.Operator = FilterOperator.Equal;
                            break;

                        case FilterOperator.LessThan:
                            binaryToken.Operator = FilterOperator.GreaterThanOrEqual;
                            break;

                        case FilterOperator.LessThanOrEqual:
                            binaryToken.Operator = FilterOperator.GreaterThan;
                            break;

                        case FilterOperator.GreaterThan:
                            binaryToken.Operator = FilterOperator.LessThanOrEqual;
                            break;

                        case FilterOperator.GreaterThanOrEqual:
                            binaryToken.Operator = FilterOperator.LessThan;
                            break;
                    }
                }
                else if (token is MethodCallToken callToken)
                {
                    switch (callToken.MethodName)
                    {
                        case nameof(FilterOperator.StartsWith):
                            callToken.MethodName = nameof(FilterOperator.NotStartsWith);
                            break;

                        case nameof(FilterOperator.Contains):
                            callToken.MethodName = nameof(FilterOperator.NotContains);
                            break;

                        case nameof(FilterOperator.EndsWith):
                            callToken.MethodName = nameof(FilterOperator.NotEndsWith);
                            break;
                    }
                }
            }

            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Type == typeof(DateTime))
            {
                //var lambda = Expression.Lambda<Func<DateTime>>(node);
                //var dateTime = lambda.Compile()();
                //      base.Visit(Expression.Constant(dateTime.Ticks));
                return node;
            }

            return base.VisitNew(node);
        }

        #endregion

    }

    class BooleanVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            //if (node.Type == typeof(bool))
            //{
            //    return Expression.MakeBinary(ExpressionType.Equal, node, Expression.Constant(true));
            //}

            return base.VisitMember(node);
        }
    }

    public class FilterDescriptor
    {
        public FilterDescriptor()
        {
            CompositionOperator = FilterOperator.And;
        }

        private FilterOperator _compositionOperator;

        public FilterOperator CompositionOperator
        {
            get => _compositionOperator;
            set
            {
                if (value != FilterOperator.And && value != FilterOperator.Or)
                    #pragma warning disable CA2208 // Instantiate argument exceptions correctly
                    throw new ArgumentOutOfRangeException();
                    #pragma warning restore CA2208 // Instantiate argument exceptions correctly

                _compositionOperator = value;
            }
        }

        public string FieldName { get; set; }
        public object Value { get; set; }
        public bool InComplete { get; set; }
        public string StringOperator { get; set; }

        public FilterOperator Operator { get; set; }

        // For demo purposes
        public override string ToString() => $"{CompositionOperator} {FieldName ?? "FieldName"} {Operator} {Value ?? "Value"}";
    }
}
