using System;
using Refit;

namespace Heartstone
{
    public interface IHeartstoneApi
    {
        [Get("/cards")]
        IObservable<string> GetCards();

        [Get("/cards/sets/{set}")]
        IObservable<CardList> GetCardSet(string set);

    }
}
