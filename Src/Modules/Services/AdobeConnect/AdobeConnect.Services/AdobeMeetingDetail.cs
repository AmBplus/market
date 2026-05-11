namespace AdobeConnectSdk;


public record AdobeMeetingDetail
{
    public required DateTime DateBegin { get; init; }
    public required DateTime DateEnd { get; init; }
    public required DateTime DateModified { get; init; }
    public required DateTime DateCreated { get; init; }
    public required DateTime DateClosed { get; init; }
    public required string ScoId { get; init; }
    public required string AccountId { get; init; }
    public required string FolderId { get; init; }
    public required string Language { get; init; }
    public required string Name { get; init; }
    public required string UrlPath { get; init; }
    public required string FullUrl { get; init; }
    public required int PassingScore { get; init; }
    public required int Duration { get; init; }
    public required int SectionCount { get; init; }
}
