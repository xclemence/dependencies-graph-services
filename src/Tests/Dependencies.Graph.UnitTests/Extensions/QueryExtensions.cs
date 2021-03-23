using System;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Neo4j.Driver;

namespace Dependencies.Graph.UnitTests.Extensions
{
    public static class QueryExtensions
    {
        public static string ValidateQuery(this Query query) => query.ValidateParameter() ?? query.ValidateSyntax()?.Message;

        public static Exception ValidateSyntax(this Query query)
        {
            var stream = CharStreams.fromString(query.Text);
            var lexer = new CypherLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new CypherParser(tokens)
            {
                BuildParseTree = true,
                TrimParseTree = true
            };

            var cypher = parser.oC_Cypher();

            return cypher.exception;
        }

        public static string ValidateParameter(this Query query)
        {
            var queryParameters = Regex.Matches(query.Text, $"\\$[[a-zA-Z0-9_-]+").Select(x => x.Value).ToList();
            var parameters = query.Parameters.Select(x => $"${x.Key}");

            var missingParamter = queryParameters.Except(parameters).ToArray();
            var notUsedParameter = parameters.Except(queryParameters).ToArray();

            if (missingParamter.Length != 0 || notUsedParameter.Length != 0)
                return $"Missing Parameters: { missingParamter.DefaultIfEmpty().Aggregate((x, y) => $"{x}{y}") }, Not used parameters: {notUsedParameter.DefaultIfEmpty().Aggregate((x, y) => $"{x}{y}")}";

            return null;
        }
    }
}
