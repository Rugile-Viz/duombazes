namespace Org.Ktu.Isk.P175B602.Autonuoma;

using System.Collections.Concurrent;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

/// <summary>
/// <para>Helper for executing MySQL queries and statements.</para>
/// <para>Static members are thread safe, instance members are not.</para>
/// </summary>
public class Sql
{
	/// <summary>
	/// Creates textual SQL quries from SqlCommand's. The code is adapted from
	/// https://stackoverflow.com/a/30630876
	/// </summary>
	private class QueryFormatter 
	{
		/// <summary>
		/// Convert value of given parameter to textual representation fit to use in SQL query.
		/// </summary>
		/// <param name="param">Parameter to convert.</param>
		/// <returns>Textual representation of parameter value.</returns>
		public string ParamValToStr(MySqlParameter param)
		{
			string strPv = "";

			switch (param.MySqlDbType)
			{
				case MySqlDbType.Text:				
				case MySqlDbType.VarChar:
				case MySqlDbType.VarString:
				case MySqlDbType.Enum:
				case MySqlDbType.Date:
				case MySqlDbType.Time:
				case MySqlDbType.Timestamp:
				case MySqlDbType.DateTime:
				case MySqlDbType.Year:
				case MySqlDbType.Newdate: 
					{
						if( param.Value == null || param.Value == DBNull.Value )
							strPv = "NULL";
						else
							strPv = "'" + param.Value.ToString().Replace("'", "''") + "'";

						break;
					}

				case MySqlDbType.Bit:
					{
						if( param.Value == null || param.Value == DBNull.Value )
							strPv = "NULL";
						else
							strPv = ((bool)param.Value == false) ? "0" : "1";

						break;
					}

				default:
					{
						if( param.Value == null || param.Value == DBNull.Value )
							strPv = "NULL";
						else
							strPv = param.Value.ToString().Replace("'", "''");
						break;
					}
			}

			return strPv;
		}

		/// <summary>
		/// Convert given MySQL/MariaDB command to SQL representation.
		/// </summary>
		/// <param name="cmd">Command to convert.</param>
		/// <returns>Textural representation of the given command.</returns>
		public string CommandToStr(MySqlCommand cmd)
		{
			string sql = cmd.CommandText;

			sql = 
				sql.Replace("\r\n", "")
				.Replace("\r", "")
				.Replace("\n", "");
			sql = Regex.Replace(sql, @"\s+", " ");

			foreach( MySqlParameter param in cmd.Parameters )
			{
				string paramName = param.ParameterName;
				string paramValue = ParamValToStr(param);
				sql = sql.Replace(paramName, paramValue);
			}

			sql = sql.Replace("= NULL", "IS NULL");
			sql = sql.Replace("!= NULL", "IS NOT NULL");

			return sql;
		}
	}

	/// <summary>
	/// Helper for extracting type attribute values from a DataRow.
	/// </summary>
	public class DataRowExtractor
	{
		/// <summary>
		/// Source row.
		/// </summary>
		private DataRow mRow;
		
		/// <summary>
		/// SQL helper.
		/// </summary>
		private Sql mSql;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="row">Source row.</param>
		public DataRowExtractor(DataRow row, Sql sql)
		{
			//validate inputs
			if( row == null )
				throw new ArgumentException("Argument 'row' is null.");

			if( sql == null )
				throw new ArgumentException("Argument 'sql' is null.");

			//store inputs
			this.mRow = row;
			this.mSql = sql;
		}

		/// <summary>
		/// Get value of given row attribute and convert it to given type T.
		/// </summary>
		/// <param name="attrName">Name of attribute to get the value from.</param>
		/// <typeparam name="T">Type to convert to.</typeparam>
		public T From<T>(string attrName)
		{
			//validate inputs
			if( attrName == null )
				throw new ArgumentException("Argument 'attrName' is null.");

			//get attribute value
			var attr = mRow[attrName];

			//convert to result type;  (T)(object) conversion is used everywhere because C# does not support
			//a direct (T) coversion from unknown type
			{
				//byte and byte?
				if( typeof(T) == typeof(byte) )
					return (T)(object)Convert.ToByte(attr);

				if( typeof(T) == typeof(byte?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToByte(it));

				//short and short?
				if( typeof(T) == typeof(short) )
					return (T)(object)Convert.ToInt16(attr);

				if( typeof(T) == typeof(short?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToInt16(it));

				//int and int?
				if( typeof(T) == typeof(int) )
					return (T)(object)Convert.ToInt32(attr);

				if( typeof(T) == typeof(int?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToInt32(it));

				//long and long?
				if( typeof(T) == typeof(long) )
					return (T)(object)Convert.ToInt64(attr);

				if( typeof(T) == typeof(long?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToInt64(it));

				//bool and bool?
				if( typeof(T) == typeof(bool) )
					return (T)(object)Convert.ToBoolean(attr);

				if( typeof(T) == typeof(bool?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToBoolean(it));

				//double and double?
				if( typeof(T) == typeof(double) )
					return (T)(object)Convert.ToDouble(attr);

				if( typeof(T) == typeof(double?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToDouble(it));

				//float and float?
				if( typeof(T) == typeof(float) )
					return (T)(object)Convert.ToDouble(attr);

				if( typeof(T) == typeof(float?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToDouble(it));

				//decimal and decimal?
				if( typeof(T) == typeof(decimal) )
					return (T)(object)Convert.ToDecimal(attr);

				if( typeof(T) == typeof(decimal?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToDecimal(it));

				//datetime and datetime?
				if( typeof(T) == typeof(DateTime) )
					return (T)(object)Convert.ToDateTime(attr);

				if( typeof(T) == typeof(DateTime?) )
					return (T)(object)mSql.AllowNull(attr, it => Convert.ToDateTime(it));

				//string
				if( typeof(T) == typeof(string) )
					return (T)(object)Convert.ToString(attr);

				//unsupported target type
				throw new Exception($"Target type '{typeof(T)}' is not supported in '<T>'.");
			}
		}
	}

	/// <summary>
	/// Helper for setting query arguments.
	/// </summary>
	public class CommandArgumentSetter
	{
		/// <summary>
		/// Target command.
		/// </summary>
		private MySqlCommand mCmd;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="cmd">Target command.</param>
		public CommandArgumentSetter(MySqlCommand cmd)
		{
			//validate inputs
			if( cmd == null )
				throw new ArgumentException("Argument 'row' is null.");

			//
			this.mCmd = cmd;
		}

		/// <summary>
		/// Add given argument with given value.
		/// </summary>
		/// <param name="argName">Name of argument to set the value for.</param>
		/// <param name="argValue">Value to set.</param>
		/// <typeparam name="T">Type of value.</typeparam>
		public void Add<T>(string argName, T argValue)
		{
			//validate inputs
			if( argName == null )
				throw new ArgumentException("Argument 'argName' is null.");

			//make a shortcut
			var pars = mCmd.Parameters;

			//set
			{
				//byte and byte?
				if( typeof(T) == typeof(byte) || typeof(T) == typeof(byte?) )
				{
					pars.Add(argName, MySqlDbType.Byte).Value = argValue;
					return;
				}

				//short and short?
				if( typeof(T) == typeof(short) || typeof(T) == typeof(short?) )
				{
					pars.Add(argName, MySqlDbType.Int16).Value = argValue;
					return;
				}

				//int and int?
				if( typeof(T) == typeof(int) || typeof(T) == typeof(int?) )
				{
					pars.Add(argName, MySqlDbType.Int32).Value = argValue;
					return;
				}

				//long and long?
				if( typeof(T) == typeof(long) || typeof(T) == typeof(long?) )
				{
					pars.Add(argName, MySqlDbType.Int64).Value = argValue;
					return;
				}

				//bool and bool?
				if( typeof(T) == typeof(bool) || typeof(T) == typeof(bool?) )
				{
					pars.Add(argName, MySqlDbType.Bit).Value = argValue;
					return;
				}

				//double and double?
				if( typeof(T) == typeof(double) || typeof(T) == typeof(double?) )
				{
					pars.Add(argName, MySqlDbType.Double).Value = argValue;
					return;
				}

				//float and float?
				if( typeof(T) == typeof(float) || typeof(T) == typeof(float?) )
				{
					pars.Add(argName, MySqlDbType.Float).Value = argValue;
					return;
				}

				//decimal and decimal?
				if( typeof(T) == typeof(decimal) || typeof(T) == typeof(decimal?) )
				{
					pars.Add(argName, MySqlDbType.Decimal).Value = argValue;
					return;
				}

				//datetime and datetime?
				if( typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?) )
				{
					pars.Add(argName, MySqlDbType.DateTime).Value = argValue;
					return;
				}

				//string
				if( typeof(T) == typeof(string) )
				{
					pars.Add(argName, MySqlDbType.VarChar).Value = argValue;
					return;
				}

				//unsupported source type
				throw new Exception($"Source type '{typeof(T)}' is not supported in 'paramValue'.");
			}
		}
	}

	/// <summary>
	/// The context local instance of this class. Set by pre-action hooks in ControllerBase.
	/// </summary>
	public static AsyncLocal<Sql> LocalInstance = new AsyncLocal<Sql>();

	/// <summary>
	/// Stores SQL queries in textual form to be passed in HTTP response through a cookie. (request-id, fifo-of(queries)).
	/// </summary>
	public static ConcurrentDictionary<String, ConcurrentQueue<String>> Queries { get; } = new ConcurrentDictionary<String, ConcurrentQueue<String>>();

	/// <summary>
	/// ID of HTTP request being serviced. Is used to correlated SQL query strings with a concrete request.
	/// </summary>
	private String mHttpRequestId;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="httpRequestId">ID of HTTP request being serviced.</param>
	public Sql(String httpRequestId) 
	{
		//validate and store inputs
		if( httpRequestId == null )
			throw new ArgumentException("Argument 'httpRequest' is null.");

		this.mHttpRequestId = httpRequestId;

		//allocate a queue for storing SQL related to given HTTP request
		Queries[httpRequestId] = new ConcurrentQueue<string>();
	}

	/// <summary>
	/// Destructor.
	/// </summary>
	~Sql() 
	{
		//release resources
		if( Queries.ContainsKey(mHttpRequestId) )
			Queries.Remove(mHttpRequestId, out _);
	}

	/// <summary>
	/// Add SQL of given command to the queries queue.
	/// </summary>
	/// <param name="cmd">Command to take SQL from.</param>
	/// <returns>SQL of the given command.</returns>
	private String RecordCommandSql(MySqlCommand cmd) 
	{
		var qf = new QueryFormatter();
		var sql = qf.CommandToStr(cmd);
		Queries[mHttpRequestId].Enqueue(sql);

		return sql;
	}

	/// <summary>
	/// Execute SELECT query.
	/// </summary>
	/// <param name="query">Query to execute.</param>
	/// <param name="args">Argument binder.</param>
	/// <returns>Rows of the result.</returns>
	public DataRowCollection Query(string query, Action<CommandArgumentSetter> args = null)
	{
		//get DB connection string from configuration
		var dbConnStr = Config.DBConnStr;

		//connect to DB and create SQL command
		using( var dbCon = new MySqlConnection(dbConnStr) )
		using( var dbCmd = new MySqlCommand(query, dbCon) )
		{
			//invoke command argument setter, if provided
			if( args != null )
			{
				var cas = new CommandArgumentSetter(dbCmd);
				args(cas);
			}

			//add SQL of the command to query strings list, check the 'sql' var if you need SQL here
			var sql = RecordCommandSql(dbCmd);

			//execute the command and get results
			dbCon.Open();
			var da = new MySqlDataAdapter(dbCmd);
			var dt = new DataTable();
			da.Fill(dt);

			//
			return dt.Rows;
		}
	}

	/// <summary>
	/// Execute INSERT statement.
	/// </summary>
	/// <param name="statement">Statement to execute.</param>
	/// <param name="args">Argument binder.</param>
	/// <returns>Autoincrementable ID of the last record created, if any.</returns>
	public long Insert(string statement, Action<CommandArgumentSetter> args = null)
	{
		//get DB connection string from configuration
		var dbConnStr = Config.DBConnStr;

		//connect to DB and create SQL command
		using( var dbCon = new MySqlConnection(dbConnStr) )
		using( var dbCmd = new MySqlCommand(statement, dbCon) )
		{
			//invoke command argument setter, if provided
			if( args != null)
			{
				var cas = new CommandArgumentSetter(dbCmd);
				args(cas);
			}

			//add SQL of the command to query strings list, check the 'sql' var if you need SQL here
			var sql = RecordCommandSql(dbCmd);

			//execute the command and get results
			dbCon.Open();
			var numRowsAffected = dbCmd.ExecuteNonQuery();

			//
			return dbCmd.LastInsertedId;
		}
	}

	/// <summary>
	/// Execute UPDATE statement.
	/// </summary>
	/// <param name="statement">Statement to execute.</param>
	/// <param name="args">Argument binder.</param>
	/// <returns>Number of rows affected.</returns>
	public int Update(string statement, Action<CommandArgumentSetter> args = null)
	{
		//get DB connection string from configuration
		var dbConnStr = Config.DBConnStr;

		//connect to DB and create SQL command
		using( var dbCon = new MySqlConnection(dbConnStr) )
		using( var dbCmd = new MySqlCommand(statement, dbCon) )
		{
			//invoke command argument setter, if provided
			if( args != null )
			{
				var cas = new CommandArgumentSetter(dbCmd);
				args(cas);
			}

			//add SQL of the command to query strings list, check the 'sql' var if you need SQL here
			var sql = RecordCommandSql(dbCmd);

			//execute the command and get results
			dbCon.Open();
			var numRowsAffected = dbCmd.ExecuteNonQuery();

			//
			return numRowsAffected;
		}
	}

	/// <summary>
	/// Execute DELETE statement.
	/// </summary>
	/// <param name="statement">Statement to execute.</param>
	/// <param name="args">Argument binder.</param>
	/// <returns>Number of rows affected.</returns>
	public int Delete(string statement, Action<CommandArgumentSetter> args = null)
	{
		//get DB connection string from configuration
		var dbConnStr = Config.DBConnStr;

		//connect to DB and create SQL command
		using( var dbCon = new MySqlConnection(dbConnStr) )
		using( var dbCmd = new MySqlCommand(statement, dbCon) )
		{
			//invoke command argument setter, if provided
			if( args != null )
			{
				var cas = new CommandArgumentSetter(dbCmd);
				args(cas);
			}

			//add SQL of the command to query strings list, check the 'sql' var if you need SQL here
			var sql = RecordCommandSql(dbCmd);

			//execute the command and get results
			dbCon.Open();
			var numRowsAffected = dbCmd.ExecuteNonQuery();

			//
			return numRowsAffected;
		}
	}

	/// <summary>
	/// Helper for converting nullable DataRow entries to expected type. Will return default
	/// value for the expected type if entry == DBNull.Value or apply given converter otherwise.
	/// </summary>
	/// <param name="entry">Entry to convert.</param>
	/// <param name="converter">Converter to apply.</param>
	/// <typeparam name="T">Expected type of entry.</typeparam>
	/// <typeparam name="T">Expected type of result.</typeparam>
	/// <returns>default(T) if entry == DBNull.Value, result of converter(entry) otherwise.</returns>
	public T AllowNull<E,T>(E entry, Func<E, T> converter) where E : class
	{
		if( entry == DBNull.Value )
			return default(T);

		return converter(entry);
	}

	/// <summary>
	/// Map given instance of DataRowCollection to List of instances of given type.
	/// </summary>
	/// <param name="rows">Collection of rows to map.</param>
	/// <param name="mapper">Mapper to apply to each row. (row-extractor, target-instance).</param>
	/// <typeparam name="T">Type of target instance.</typeparam>
	/// <returns>A list corresponding to given collection of rows.</returns>
	public List<T> MapAll<T>(DataRowCollection rows, Action<DataRowExtractor, T> mapper) where T : class, new()
	{
		//create result store
		var list = new List<T>(rows.Count);

		//map all rows
		foreach(DataRow row in rows)
		{
			//create new instance of a target type to be mapped to current row
			var target = new T();

			//map current row
			var extr = new DataRowExtractor(row, this);
			mapper(extr, target);

			//store result
			list.Add(target);
		}

		//
		return list;
	}

	/// <summary>
	/// Map first row in a given instance of DataRowCollection to an instance of a given type.
	/// </summary>
	/// <param name="rows">Collection of rows to map the first row from.</param>
	/// <param name="mapper">Mapper to apply to the first row. (row-extractor, target-instance).</param>
	/// <returns>An instance corresponding to the first row.</returns>
	/// <typeparam name="T">Type of target instance.</typeparam>
	public T MapOne<T>(DataRowCollection rows, Action<DataRowExtractor, T> mapper) where T : class, new()
	{	
		//nothing to map? throw exception
		if( rows.Count == 0 )
			throw new ArgumentException("There are no rows in argument 'rows'.");

		//create new instance of a target type to be mapped to
		var target = new T();

		//map first row
		var extr = new DataRowExtractor(rows[0], this);
		mapper(extr, target);

		//
		return target;
	}
}
