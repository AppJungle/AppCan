using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using StockTraderRI.Infrastructure.Models;

namespace StockTraderRI.Infrastructure
{
    public class SetNewsArticleEvent : CompositePresentationEvent<NewsArticle>
    {
    }
}
