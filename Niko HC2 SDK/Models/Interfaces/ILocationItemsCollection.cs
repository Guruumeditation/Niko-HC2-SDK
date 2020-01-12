using System.Collections.Generic;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface ILocationItemsCollection
    {
        string Id { get; set; }
        List<ILocationItem> Items { get; set; }
    }
}