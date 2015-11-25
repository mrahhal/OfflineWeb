using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OfflineWeb.Visitors
{
	public interface IVisitor
	{
		/// <summary>
		/// Gets the nodes we're interested in visiting.
		/// </summary>
		NodeKind InterestingNodes { get; }

		/// <summary>
		/// Called to visit an interesting node.
		/// </summary>
		/// <param name="context">The visiting context.</param>
		/// <param name="node">The node that we're visiting.</param>
		/// <returns>The same node if no transformation is needed, a newly created one if transformation has been done, or null to remove the node.</returns>
		Task<HtmlNode> VisitAsync(VisitingContext context, HtmlNode node);
	}
}