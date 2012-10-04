/*

Copyright dotNetRDF Project 2009-12
dotnetrdf-develop@lists.sf.net

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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Writing.Formatting;

namespace VDS.RDF.Test.Sparql
{
    [TestClass]
    public class GroupByTests
    {
        [TestMethod]
        public void SparqlGroupByInSubQuery()
        {
            String query = "SELECT ?s WHERE {{SELECT ?s WHERE {?s ?p ?o} GROUP BY ?s}} GROUP BY ?s";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);
        }

        [TestMethod]
        public void SparqlGroupByAssignmentSimple()
        {
            String query = "SELECT ?x WHERE { ?s ?p ?o } GROUP BY ?s AS ?x";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);

            SparqlFormatter formatter = new SparqlFormatter();
            Console.WriteLine(formatter.Format(q));
            Console.WriteLine();

            QueryableGraph g = new QueryableGraph();
            FileLoader.Load(g, "InferenceTest.ttl");

            Object results = g.ExecuteQuery(q);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                TestTools.ShowResults(rset);

                Assert.IsTrue(rset.All(r => r.HasValue("x") && !r.HasValue("s")), "All Results should have a ?x variable and no ?s variable");
            }
            else
            {
                Assert.Fail("Didn't get a Result Set as expected");
            }
        }

        [TestMethod]
        public void SparqlGroupByAssignmentSimple2()
        {
            String query = "SELECT ?x (COUNT(?p) AS ?predicates) WHERE { ?s ?p ?o } GROUP BY ?s AS ?x";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);

            SparqlFormatter formatter = new SparqlFormatter();
            Console.WriteLine(formatter.Format(q));
            Console.WriteLine();

            QueryableGraph g = new QueryableGraph();
            FileLoader.Load(g, "InferenceTest.ttl");

            Object results = g.ExecuteQuery(q);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                TestTools.ShowResults(rset);

                Assert.IsTrue(rset.All(r => r.HasValue("x") && !r.HasValue("s") && r.HasValue("predicates")), "All Results should have a ?x and ?predicates variables and no ?s variable");
            }
            else
            {
                Assert.Fail("Didn't get a Result Set as expected");
            }
        }

        [TestMethod]
        public void SparqlGroupByAssignmentExpression()
        {
            String query = "SELECT ?s ?sum WHERE { ?s ?p ?o } GROUP BY ?s (1 + 2 AS ?sum)";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);

            SparqlFormatter formatter = new SparqlFormatter();
            Console.WriteLine(formatter.Format(q));
            Console.WriteLine();

            QueryableGraph g = new QueryableGraph();
            FileLoader.Load(g, "InferenceTest.ttl");

            Object results = g.ExecuteQuery(q);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                TestTools.ShowResults(rset);

                Assert.IsTrue(rset.All(r => r.HasValue("s") && r.HasValue("sum")), "All Results should have a ?s and a ?sum variable");
            }
            else
            {
                Assert.Fail("Didn't get a Result Set as expected");
            }
        }

        [TestMethod]
        public void SparqlGroupByAssignmentExpression2()
        {
            String query = "SELECT ?lang (SAMPLE(?o) AS ?example) WHERE { ?s ?p ?o . FILTER(ISLITERAL(?o)) } GROUP BY (LANG(?o) AS ?lang)";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);

            SparqlFormatter formatter = new SparqlFormatter();
            Console.WriteLine(formatter.Format(q));
            Console.WriteLine();

            QueryableGraph g = new QueryableGraph();
            UriLoader.Load(g, new Uri("http://dbpedia.org/resource/Southampton"));

            Object results = g.ExecuteQuery(q);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                TestTools.ShowResults(rset);

                Assert.IsTrue(rset.All(r => r.HasValue("lang") && r.HasValue("example")), "All Results should have a ?lang and a ?example variable");
            }
            else
            {
                Assert.Fail("Didn't get a Result Set as expected");
            }
        }

        [TestMethod]
        public void SparqlGroupByAssignmentExpression3()
        {
            String query = "SELECT ?lang (SAMPLE(?o) AS ?example) WHERE { ?s ?p ?o . FILTER(ISLITERAL(?o)) } GROUP BY (LANG(?o) AS ?lang) HAVING LANGMATCHES(?lang, \"*\")";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);

            SparqlFormatter formatter = new SparqlFormatter();
            Console.WriteLine(formatter.Format(q));
            Console.WriteLine();

            QueryableGraph g = new QueryableGraph();
            UriLoader.Load(g, new Uri("http://dbpedia.org/resource/Southampton"));

            Object results = g.ExecuteQuery(q);
            if (results is SparqlResultSet)
            {
                SparqlResultSet rset = (SparqlResultSet)results;
                TestTools.ShowResults(rset);

                Assert.IsTrue(rset.All(r => r.HasValue("lang") && r.HasValue("example")), "All Results should have a ?lang and a ?example variable");
            }
            else
            {
                Assert.Fail("Didn't get a Result Set as expected");
            }
        }

        [TestMethod]
        public void SparqlGroupBySample()
        {
            String query = "SELECT ?s (SAMPLE(?o) AS ?object) WHERE {?s ?p ?o} GROUP BY ?s";
            SparqlQueryParser parser = new SparqlQueryParser();
            SparqlQuery q = parser.ParseFromString(query);
        }

        [TestMethod]
        public void SparqlGroupByAggregateEmptyGroup()
        {
            String query = @"PREFIX ex: <http://example.com/>
SELECT ?x (MAX(?value) AS ?max)
WHERE {
	?x ex:p ?value
} GROUP BY ?x";

            SparqlQuery q = new SparqlQueryParser().ParseFromString(query);
            LeviathanQueryProcessor processor = new LeviathanQueryProcessor(new TripleStore());
            SparqlResultSet results = processor.ProcessQuery(q) as SparqlResultSet;
            if (results == null) Assert.Fail("Null results");

            Assert.IsFalse(results.IsEmpty, "Results should not be empty");
            Assert.AreEqual(1, results.Count, "Should be a single result");
            Assert.IsTrue(results.First().All(kvp => kvp.Value == null), "Should be no bound values");
        }

        [TestMethod]
        public void SparqlGroupByRefactor1()
        {
            String query = @"BASE <http://www.w3.org/2001/sw/DataAccess/tests/data-r2/dataset/manifest#>
PREFIX : <http://example/>

SELECT * FROM <http://data-g3-dup.ttl>
FROM NAMED <http://data-g3.ttl>
WHERE 
{ 
  ?s ?p ?o . 
  GRAPH ?g { ?s ?q ?v . } 
}";

            String data = @"_:x <http://example/p> ""1""^^<http://www.w3.org/2001/XMLSchema#integer> <http://data-g3-dup.ttl> .
_:a <http://example/p> ""9""^^<http://www.w3.org/2001/XMLSchema#integer> <http://data-g3-dup.ttl> .
_:x <http://example/p> ""1""^^<http://www.w3.org/2001/XMLSchema#integer> <http://data-g3.ttl> .
_:a <http://example/p> ""9""^^<http://www.w3.org/2001/XMLSchema#integer> <http://data-g3.ttl> .";

            TripleStore store = new TripleStore();
            StringParser.ParseDataset(store, data, new NQuadsParser());

            SparqlResultSet results = store.ExecuteQuery(query) as SparqlResultSet;
            Assert.IsNotNull(results);
            Assert.AreEqual(6, results.Variables.Count());
        }
    }
}
