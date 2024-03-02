﻿namespace Sinfonia.Implementations
{
    internal class ScoreDocumentStyleProvider : IScoreDocumentStyleProvider
    {
        public IEnumerable<ScoreDocumentStyle> GetStyles()
        {
            return new List<ScoreDocumentStyle>()
            {
                ScoreDocumentStyle.Create(e =>
                {
                    e.ScoreDocumentLayoutFactory = new Func<IScoreDocument, ScoreDocumentLayout>(e => new ScoreDocumentLayout()
                    {
                        BreakSystem = measures => measures.Count() > 4
                    });
                })
            };
        }
    }
}