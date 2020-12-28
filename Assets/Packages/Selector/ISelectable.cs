using System;

public interface ISelectable : IHighlightable
{
    void OnSelected();
    void OnDeselected();
}