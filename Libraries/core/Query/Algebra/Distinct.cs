﻿/*

Copyright Robert Vesse 2009-10
rvesse@vdesign-studios.com

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.RDF.Query.Algebra
{
    /// <summary>
    /// Represents a Distinct modifier on a SPARQL Query
    /// </summary>
    public class Distinct : ISparqlAlgebra
    {
        private ISparqlAlgebra _pattern;

        /// <summary>
        /// Creates a new Distinct Modifier
        /// </summary>
        /// <param name="pattern">Pattern</param>
        public Distinct(ISparqlAlgebra pattern)
        {
            this._pattern = pattern;
        }

        /// <summary>
        /// Evaluates the Distinct Modifier
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <returns></returns>
        public BaseMultiset Evaluate(SparqlEvaluationContext context)
        {
            context.InputMultiset = this._pattern.Evaluate(context);

            if (context.InputMultiset is IdentityMultiset || context.InputMultiset is NullMultiset)
            {
                context.OutputMultiset = context.InputMultiset;
                return context.OutputMultiset;
            }
            else
            {
                context.OutputMultiset = new Multiset(context.InputMultiset.Variables);
                IEnumerable<Set> sets = context.InputMultiset.Sets.Distinct();
                foreach (Set s in context.InputMultiset.Sets.Distinct())
                {
                    context.OutputMultiset.Add(s);
                }
                return context.OutputMultiset;
            }
        }

        /// <summary>
        /// Gets the Variables used in the Algebra
        /// </summary>
        public IEnumerable<String> Variables
        {
            get
            {
                return this._pattern.Variables;
            }
        }

        /// <summary>
        /// Gets the String representation of the Algebra
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Distinct(" + this._pattern.ToString() + ")";
        }
    }

    /// <summary>
    /// Represents a Reduced modifier on a SPARQL Query
    /// </summary>
    public class Reduced : ISparqlAlgebra
    {
        private ISparqlAlgebra _pattern;

        /// <summary>
        /// Creates a new Reduced Modifier
        /// </summary>
        /// <param name="pattern">Pattern</param>
        public Reduced(ISparqlAlgebra pattern)
        {
            this._pattern = pattern;
        }

        /// <summary>
        /// Evaluates the Reduced Modifier
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <returns></returns>
        public BaseMultiset Evaluate(SparqlEvaluationContext context)
        {
            context.InputMultiset = this._pattern.Evaluate(context);

            if (context.InputMultiset is IdentityMultiset || context.InputMultiset is NullMultiset)
            {
                context.OutputMultiset = context.InputMultiset;
                return context.OutputMultiset;
            }
            else
            {
                if (context.Query.Limit > 0)
                {
                    context.OutputMultiset = new Multiset(context.InputMultiset.Variables);
                    foreach (Set s in context.InputMultiset.Sets.Distinct())
                    {
                        context.OutputMultiset.Add(s);
                    }
                }
                else
                {
                    context.OutputMultiset = context.InputMultiset;
                }
                return context.OutputMultiset;
            }
        }

        /// <summary>
        /// Gets the Variables used in the Algebra
        /// </summary>
        public IEnumerable<String> Variables
        {
            get
            {
                return this._pattern.Variables.Distinct();
            }
        }

        /// <summary>
        /// Gets the String representation of the Algebra
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Reduced(" + this._pattern.ToString() + ")";
        }
    }
}
