namespace tNav.FakeConsole;

    public record Seans(string? Query)
    {
        public List<Response> Responses { get; set; } = [];

    }
