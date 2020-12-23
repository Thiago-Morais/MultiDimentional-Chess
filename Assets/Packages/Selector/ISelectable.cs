public interface ISelectable
{
    bool Selected { get; set; }
    void OnSelected();
    void OnDeselected();
}