using System.Threading.Tasks;

namespace UEExplorer.Framework
{
    public interface IContextListener
    {
        /// <summary>
        /// Provides the opportunity to accept or deny a context change event.
        /// </summary>
        /// <param name="context">The context info to accept.</param>
        /// <returns>True if the context is acceptable.</returns>
        bool CanAccept(ContextInfo context);

        /// <summary>
        /// Executed if the context was accepted, this method is expected to apply the context change itself.
        /// </summary>
        /// <param name="context">The context info to accept.</param>
        /// <returns>True if the context was successfully accepted.</returns>
        Task<bool> Accept(ContextInfo context);
    }
}
