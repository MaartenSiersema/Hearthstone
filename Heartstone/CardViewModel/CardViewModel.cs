using System;
using ReactiveUI;
namespace Heartstone
{
    public class CardViewModel : ReactiveObject
    {
        public readonly Card Card;

        public string DescriptionLabel => Card.Text ?? string.Empty;

        public CardViewModel(Card card)
        {
            Card = card;
        }
    }
}
