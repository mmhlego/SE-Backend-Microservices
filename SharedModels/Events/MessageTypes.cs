namespace SharedModels.Events
{
	public enum MessageTypes
	{
		ProductFinished, // Message to seller + bookmarked customers
		ProductAvailable, // Message to bookmarked customers
		SaleFinished, // Message to seller
		SaleAvailable, // Message to seller + bookmarked customers
		Festival, // Message to all customers
	}
}