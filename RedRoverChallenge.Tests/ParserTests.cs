namespace RedRoverChallenge.Tests
{
    public class ParserTests
    {
        private static string Normalize(string s) => s.ReplaceLineEndings("\n");

        [Fact]
        public void SampleInputUnsorted()
        {
            var input = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
            var expected = """
                - id
                - name
                - email
                - type
                  - id
                  - name
                  - customFields
                    - c1
                    - c2
                    - c3
                - externalId
                """;

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void SampleInputSorted()
        {
            var input = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
            var expected = """
                - email
                - externalId
                - id
                - name
                - type
                  - customFields
                    - c1
                    - c2
                    - c3
                  - id
                  - name
                """;

            var output = Parser.Parse(input, sorted: true);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void SingleField()
        {
            var input = "(id)";
            var expected = "- id";

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void SeveralFields()
        {
            var input = "(id, name, email)";
            var expected = """
                - id
                - name
                - email
                """;

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void WhiteSpaceIgnored()
        {
            var input = "(  id ,   type ( a , b )  )";
            var expected = """
                - id
                - type
                  - a
                  - b
                """;

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void FieldNamesWithSpacesOrPunctuation()
        {
            var input = "(first name, user.name, 10%)";
            var expected = """
                - first name
                - user.name
                - 10%
                """;

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void RedundantParentheses()
        {
            var input = "(((((a)))))";
            var expected = """
                - a
                """;

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void StrayCommas()
        {
            var input = "(a,,b,c,)";
            var expected = """
                - a
                - b
                - c
                """;

            var output = Parser.Parse(input);

            Assert.Equal(Normalize(expected), output);
        }

        [Fact]
        public void UnclosedParenthesisThrowsException()
        {
            var input = "(a,b";

            Assert.Throws<FormatException>(() => Parser.Parse(input));
        }

        [Fact]
        public void ParenthesisNeverOpenedThrowsException()
        {
            var input = "a,b)";

            Assert.Throws<FormatException>(() => Parser.Parse(input));
        }
    }
}