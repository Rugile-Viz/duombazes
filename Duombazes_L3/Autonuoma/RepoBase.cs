namespace Org.Ktu.Isk.P175B602.Autonuoma.Repositories;

using Org.Ktu.Isk.P175B602.Autonuoma.Models;



/// <summary>
/// Base class for repository classes. Used to simplify retrieval of proper instance of SQL helper.
/// </summary>
public class RepoBase 
{
    /// <summary>
	/// SQL helper.
	/// </summary>
	protected static Sql Sql {
        get {
            return Sql.LocalInstance.Value;
        }
    }
}