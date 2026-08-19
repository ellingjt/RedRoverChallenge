namespace RedRoverChallenge
{
    public static class Parser
    {
        private const int MaxNestingDepth = 100;
        private const char OpenChar = '(';
        private const char CloseChar = ')';

        // Class to hold the field name and children
        private class Field
        {
            public Field(string name, List<Field>? children = null)
            {
                Name = name;
                Children = children ?? new List<Field>();
            }

            public string Name { get; }
            public List<Field> Children { get; }
        }

        public static string Parse(string input, bool sorted = false)
        {
            // Validate input for depth and parenthesis matching
            Validate(input);

            // Build the tree of fields from input
            var fields = BuildTree(input);

            if (sorted)
            {
                fields = Sort(fields);
            }

            // Add all of the lines to the list for return and output
            var lines = new List<string>();
            AddLines(fields, 0, lines);

            return string.Join("\n", lines);
        }

        private static void Validate(string input)
        {
            var depth = 0;

            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == OpenChar)
                {
                    depth++;

                    if (depth > MaxNestingDepth)
                    {
                        throw new FormatException($"Input exceeded max nesting depth. Max: {MaxNestingDepth}");
                    }
                }
                else if (input[i] == CloseChar)
                {
                    depth--;

                    if (depth < 0)
                    {
                        throw new FormatException($"Input has mismatched parentheses at position {i}.");
                    }
                }
            }

            if (depth > 0)
            {
                throw new FormatException("Missing closing parentheses.");
            }
        }

        private static List<Field> BuildTree(string input)
        {
            var root = new List<Field>();

            // Using a stack to keep track of each level of nesting
            var openGroups = new Stack<List<Field>>();
            openGroups.Push(root);

            var start = 0;

            for (var i = 0; i < input.Length; i++)
            {
                var currentCharacter = input[i];

                // Skip characters that aren't parens or comma
                if (currentCharacter != OpenChar && currentCharacter != CloseChar && currentCharacter != ',')
                {
                    continue;
                }

                var name = input.Substring(start, i - start).Trim();
                start = i + 1;

                // Fields at the current level
                var currentFields = openGroups.Peek();

                if (currentCharacter == OpenChar)
                {
                    // New level, so we need to add children
                    if (name.Length > 0)
                    {
                        // Now that we have a name, it owns a new group, so put its children into a new level
                        var parent = new Field(name);
                        currentFields.Add(parent);
                        openGroups.Push(parent.Children);
                    }
                    else
                    {
                        // End of input, add current fields
                        openGroups.Push(currentFields);
                    }
                }
                else
                {
                    AddField(currentFields, name);

                    if (currentCharacter == CloseChar)
                    {
                        // Level is now closed
                        openGroups.Pop();
                    }
                }

            }

            // Last field needs to be added to the root
            AddField(root, input.Substring(start).Trim());

            return root;
        }

        private static void AddField(List<Field> fields, string name)
        {
            if (name.Length > 0)
            {
                fields.Add(new Field(name));
            }
        }

        // Recursively sort fields and children
        private static List<Field> Sort(List<Field> fields)
        {
            return fields
                .OrderBy(field => field.Name)
                .Select(field => new Field(field.Name, Sort(field.Children)))
                .ToList();
        }

        // Recursive method to crawl the tree and add lines for output
        private static void AddLines(List<Field> fields, int depth, List<string> lines)
        {
            var indent = new string(' ', depth * 2);

            foreach (var field in fields)
            {
                lines.Add($"{indent}- {field.Name}");
                AddLines(field.Children, depth + 1, lines);
            }
        }

    }
}
