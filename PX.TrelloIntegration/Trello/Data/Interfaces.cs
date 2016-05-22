namespace PX.TrelloIntegration.Trello
{
    public interface ITrelloObject
    {
        string Id { get; set; }
    }

    public interface ITrelloOrganizationObject
    {
        string OrganizationId { get; set; }
    }
}
