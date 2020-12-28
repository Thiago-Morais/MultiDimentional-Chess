using UnityEngine;

public interface IHighlightable
{
    Highlight Highlight { get; set; }
}
public interface IHighlighter
{
    HighlightType HighlightType { get; set; }
}