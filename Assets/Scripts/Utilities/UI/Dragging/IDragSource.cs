namespace Assets.Scripts.Utilities.UI.Dragging
{
    /// <summary>
    /// Components that implement this interfaces can act as the source for
    /// dragging a `DragItem`.
    /// </summary>
    /// <typeparam name="T">The type that represents the item being dragged.</typeparam>
    public interface IDragSource<out T> where T : class
    {
        /// <summary>
        /// What item type currently resides in this source?
        /// </summary>
        T GetItem();

        /// <summary>
        /// What is the quantity of items in this source?
        /// </summary>
        int GetNumber();

        /// <summary>
        /// Remove a given number of items from the source.
        /// </summary>
        /// <param name="number">
        /// This should never exceed the number returned by `GetNumber`.
        /// </param>
        /// /// <param name="swapAttempt">
        /// Lets method know that another item is intended to be added to the slot
        /// so don't fire the EquipmentChanged event yet.
        /// </param>
        void RemoveItems(int number, bool swapAttempt);
    }
}