using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;
using System.Text;

namespace GridView_Sample
{
    /// <summary>
    /// DAO：スタッフ（Staff）
    /// </summary>
    public class StaffDao
    {
        /// <summary>
        /// ORDER BY 指定用のフィールド名のリスト
        /// </summary>
        public static String[] FieldNames = { "StaffId", "Account", "Password", "DisplayName", "StaffType", "UpdateDate", "UpdateStaffId", "DeleteDate" };

        /// <summary>
        /// スタッフ（Staff）テーブルから1レコードを論理削除する
        /// </summary>
        /// <param name="staffId">論理削除するレコードのスタッフID</param>
        /// <param name="deleteStaffId">レコードを論理削除するスタッフのスタッフID</param>
        /// <returns>論理削除されたレコードの数</returns>
        public static int Delete(int staffId, int deleteStaffId)
        {
            int result = 0;

            // データベースに接続する
            String connectionString = ConfigurationManager.ConnectionStrings["ASPNETConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    // スタッフ（Staff）テーブルの1レコードに対して「削除日時」を設定するSQL文の実行を準備する
                    string deleteQuery = "UPDATE [Staff] SET [UpdateDate] = CURRENT_TIMESTAMP, [UpdateStaffId] = @UpdateStaffId, [DeleteDate] = CURRENT_TIMESTAMP WHERE [StaffId] = @StaffId";
                    SqlCommand sqlDeleteCommand = new SqlCommand(deleteQuery, sqlConnection, sqlTransaction);
                    sqlDeleteCommand.Parameters.Add("@UpdateStaffId", SqlDbType.Int).Value = deleteStaffId;
                    sqlDeleteCommand.Parameters.Add("@StaffId", SqlDbType.Int).Value = staffId;

                    // SQL文を実行して「影響を受けた＝更新された」行数を取得する
                    result = sqlDeleteCommand.ExecuteNonQuery();
                    if (0 < result)
                    {
                        // 現時点での「システム管理者の残り人数」を確認する
                        string countQuery = "SELECT COUNT(*) FROM [Staff] WHERE [StaffType] = @StaffType AND [DeleteDate] IS NULL";
                        SqlCommand sqlCountCommand = new SqlCommand(countQuery, sqlConnection, sqlTransaction);
                        sqlCountCommand.Parameters.Add("@StaffType", SqlDbType.TinyInt).Value = (byte)StaffType.ADMIN;
                        int numAdmin = (int)sqlCountCommand.ExecuteScalar();
                        if (numAdmin <= 0)
                        {
                            throw new Exception("システムには1名以上のシステム管理者が必要です。");
                        }
                    }

                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    sqlTransaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// スタッフ（Staff）テーブルの結果セットから1レコードをフェッチする
        /// </summary>
        /// <param name="reader">結果セットの読み取りオブジェクト</param>
        /// <returns>スタッフDTO</returns>
        protected static StaffDto Fetch(SqlDataReader reader)
        {
            // 結果セットから1レコードを読み取ってDTOに格納して返す
            StaffDto Staff = new StaffDto();
            Staff.StaffId = (int)reader["StaffId"];
            Staff.Account = reader["Account"] as string;
            Staff.Password = reader["Password"] as string;
            Staff.DisplayName = reader["DisplayName"] as string;
            Staff.StaffType = StaffType.MEMBER;
            int staffTypeInt = (byte)reader["StaffType"];
            if (true == Enum.TryParse<StaffType>(staffTypeInt.ToString(), out StaffType staffType))
            {
                if (Enum.IsDefined(typeof(StaffType), staffType))
                {
                    Staff.StaffType = staffType;
                }
            }
            Staff.UpdateDate = reader["UpdateDate"] as DateTime?;
            Staff.UpdateStaffId = (int)reader["UpdateStaffId"];
            Staff.DeleteDate = reader["DeleteDate"] as DateTime?;
            return Staff;
        }

        /// <summary>
        /// スタッフ（Staff）テーブルに1レコードを挿入する
        /// </summary>
        /// <param name="Staff">挿入するレコードのデータ</param>
        /// <param name="createStaffId">レコードを挿入するスタッフのスタッフID</param>
        /// <returns>挿入されたレコードの数</returns>
        /// <exception cref="Exception">アカウント名が重複している</exception>
        public static int Insert(StaffDto Staff, int createStaffId)
        {
            int result = 0;

            // データベースに接続する
            String connectionString = ConfigurationManager.ConnectionStrings["ASPNETConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // スタッフ（Staff）テーブルに1レコードを挿入するSQL文の実行準備を行う
                StringBuilder insertQuery = new StringBuilder("INSERT INTO [Staff]");
                insertQuery.Append(" ( [Account], [Password], [DisplayName], [StaffType], [UpdateStaffId] )");
                insertQuery.Append(" VALUES");
                insertQuery.Append(" ( @Account, @Password, @DisplayName, @StaffType, @UpdateStaffId )");
                SqlCommand sqlInsertCommand = new SqlCommand(insertQuery.ToString(), sqlConnection);

                // 実行準備したSQL文の各SQLパラメータに対してDTOに格納されたフィールドの値を設定する
                sqlInsertCommand.Parameters.Add("@Account", SqlDbType.NVarChar).Value = Staff.Account;
                sqlInsertCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Staff.Password;
                sqlInsertCommand.Parameters.Add("@DisplayName", SqlDbType.NVarChar).Value = Staff.DisplayName;
                sqlInsertCommand.Parameters.Add("@StaffType", SqlDbType.TinyInt).Value = (byte)Staff.StaffType;
                sqlInsertCommand.Parameters.Add("@UpdateStaffId", SqlDbType.Int).Value = createStaffId;

                try
                {
                    // SQL文を実行して「影響を受けた＝挿入された」行数を取得する
                    result = sqlInsertCommand.ExecuteNonQuery();
                    if (0 < result)
                    {
                        // 新たに生成されたID値を取得してスタッフ情報のID値として返却する
                        SqlCommand sqlSelectCommand = new SqlCommand("SELECT CAST(@@IDENTITY AS INT) AS [new_id]", sqlConnection);
                        using (SqlDataReader reader = sqlSelectCommand.ExecuteReader())
                        {
                            if (true == reader.Read())
                            {
                                int ord = reader.GetOrdinal("new_id");
                                Staff.StaffId = reader.GetInt32(ord);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // 一意制約違反が発生している…
                    if (2627 == ex.Number)
                    {
                        throw new Exception("同一のログインアカウントが存在しています。", ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// スタッフ（Staff）テーブルから1レコードを取得する
        /// </summary>
        /// <param name="staffId">取得するレコードのスタッフID</param>
        /// <returns>取得できたレコードを格納したDTO／取得できない場合にはnull</returns>
        public static StaffDto Select(int staffId)
        {
            StaffDto result = null;

            // データベースに接続する
            String connectionString = ConfigurationManager.ConnectionStrings["ASPNETConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // スタッフ（Staff）テーブルから1レコードを取得するSQL文を実行する
                string selectQuery = "SELECT [StaffId], [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] FROM [Staff] WHERE [StaffId] = @StaffId";
                SqlCommand sqlSelectCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlSelectCommand.Parameters.Add("@StaffId", SqlDbType.Int).Value = staffId;
                using (SqlDataReader reader = sqlSelectCommand.ExecuteReader())
                {
                    if (true == reader.Read())
                    {
                        result = StaffDao.Fetch(reader);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// スタッフ（Staff）テーブルからレコードのリストを取得する
        /// </summary>
        /// <param name="account">アカウント名で検索する場合に指定する</param>
        /// <param name="staffType">スタッフ種別で検索する場合に指定する</param>
        /// <param name="displayName">表示名で検索する場合に指定する</param>
        /// <returns>取得できたレコードのリスト</returns>
        public static List<StaffDto> Select(string account = null, StaffType? staffType = null, string displayName = null, int orderField = 1, bool orderDirectionAscending = true)
        {
            List<StaffDto> results = new List<StaffDto>();

            // データベースに接続する
            String connectionString = ConfigurationManager.ConnectionStrings["ASPNETConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // スタッフ（Staff）テーブルからレコードのリストを取得するSQL文の実行準備を行う
                StringBuilder selectQuery = new StringBuilder("SELECT [StaffId], [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] FROM [Staff]");
                List<string> whereConds = new List<string>();
                if (true != String.IsNullOrEmpty(account))
                {
                    whereConds.Add("[Account] LIKE @Account");
                }
                if (null != staffType)
                {
                    whereConds.Add("[StaffType] = @StaffType");
                }
                if (true != String.IsNullOrEmpty(displayName))
                {
                    whereConds.Add("[DisplayName] LIKE @DisplayName");
                }
                whereConds.Add("[DeleteDate] IS NULL");
                if (0 < whereConds.Count)
                {
                    selectQuery.Append(" WHERE " + String.Join(" AND ", whereConds));
                }
                selectQuery.Append(String.Format(" ORDER BY [{0}] {1}", StaffDao.FieldNames[orderField], (true == orderDirectionAscending) ? "ASC" : "DESC"));
                SqlCommand sqlSelectCommand = new SqlCommand(selectQuery.ToString(), sqlConnection);

                // SQLパラメータオブジェクトを生成してパラメータに値を設定する
                if (true != String.IsNullOrEmpty(account))
                {
                    sqlSelectCommand.Parameters.Add("@Account", SqlDbType.NVarChar).Value = String.Format("%{0}%", account);
                }
                if (null != staffType)
                {
                    sqlSelectCommand.Parameters.Add("@StaffType", SqlDbType.TinyInt).Value = (byte)staffType;
                }
                if (true != String.IsNullOrEmpty(displayName))
                {
                    sqlSelectCommand.Parameters.Add("@DisplayName", SqlDbType.NVarChar).Value = String.Format("%{0}%", displayName);
                }

                // SQL文を実行して、結果セットから読み取ったレコードをDTOに格納した上で値返却用のリストに格納する
                using (SqlDataReader reader = sqlSelectCommand.ExecuteReader())
                {
                    while (true == reader.Read())
                    {
                        StaffDto result = StaffDao.Fetch(reader);
                        results.Add(result);
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// スタッフ（Staff）テーブルから「アカウント名」の一致するレコードを取得する（削除されていないこと！）
        /// </summary>
        /// <param name="account">取得するレコードのアカウント名</param>
        /// <returns>取得できたデータを格納したDTO／取得できない場合にはnull</returns>
        public static StaffDto SelectByAccount(string account)
        {
            StaffDto result = null;

            // データベースに接続する
            String connectionString = ConfigurationManager.ConnectionStrings["ASPNETConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                // スタッフ（Staff）テーブルから「アカウント名」の一致する有効なレコードを取得するSQL文を実行する
                string selectQuery = "SELECT [StaffId], [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] FROM [Staff] WHERE [Account] = @Account AND [DeleteDate] IS NULL";
                SqlCommand sqlSelectCommand = new SqlCommand(selectQuery, sqlConnection);
                sqlSelectCommand.Parameters.Add("@Account", SqlDbType.NVarChar).Value = account;
                using (SqlDataReader reader = sqlSelectCommand.ExecuteReader())
                {
                    if (true == reader.Read())
                    {
                        result = StaffDao.Fetch(reader);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// スタッフ（Staff）テーブルのレコードを更新する
        /// </summary>
        /// <param name="Staff">更新するレコードのデータ</param>
        /// <param name="staffId">更新するレコードのスタッフID</param>
        /// <param name="updateStaffId">レコードを更新するスタッフのスタッフID</param>
        /// <returns>更新できたレコードの数</returns>
        public static int Update(StaffDto Staff, int staffId, int updateStaffId)
        {
            int result = 0;

            // データベースに接続する
            String connectionString = ConfigurationManager.ConnectionStrings["ASPNETConnectionString"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    // スタッフ（Staff）テーブルのレコードを更新するSQL文を実行する準備を行う
                    string updateQuery = "UPDATE [Staff] SET [Account] = @Account, [Password] = @Password, [DisplayName] = @DisplayName, [StaffType] = @StaffType, [UpdateDate] = CURRENT_TIMESTAMP, [UpdateStaffId] = @UpdateStaffId WHERE [StaffId] = @StaffId";
                    SqlCommand sqlUpdateCommand = new SqlCommand(updateQuery, sqlConnection, sqlTransaction);

                    // 実行準備したSQL文の各SQLパラメータに対して必要な値を設定する
                    sqlUpdateCommand.Parameters.Add("@Account", SqlDbType.NVarChar).Value = Staff.Account;
                    sqlUpdateCommand.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Staff.Password;
                    sqlUpdateCommand.Parameters.Add("@DisplayName", SqlDbType.NVarChar).Value = Staff.DisplayName;
                    sqlUpdateCommand.Parameters.Add("@StaffType", SqlDbType.TinyInt).Value = (byte)Staff.StaffType;
                    sqlUpdateCommand.Parameters.Add("@UpdateStaffId", SqlDbType.Int).Value = updateStaffId;
                    sqlUpdateCommand.Parameters.Add("@StaffId", SqlDbType.Int).Value = staffId;

                    // SQL文を実行して「影響を受けた＝更新された」行数を取得する
                    result = sqlUpdateCommand.ExecuteNonQuery();
                    if (0 < result)
                    {
                        // 現時点での「システム管理者の残り人数」を確認する
                        string countQuery = "SELECT COUNT(*) FROM [Staff] WHERE [StaffType] = @StaffType AND [DeleteDate] IS NULL";
                        SqlCommand sqlCountCommand = new SqlCommand(countQuery, sqlConnection, sqlTransaction);
                        sqlCountCommand.Parameters.Add("@StaffType", SqlDbType.TinyInt).Value = (byte)StaffType.ADMIN;
                        int numAdmin = (int)sqlCountCommand.ExecuteScalar();
                        if (numAdmin <= 0)
                        {
                            throw new Exception("システムには1名以上のシステム管理者が必要です。");
                        }
                    }

                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    sqlTransaction.Rollback();
                    throw;
                }
            }

            return result;
        }
    }
}