using System;

namespace GridView_Sample
{
    /// <summary>
    /// DTO：スタッフマスタ（Staff）
    /// </summary>
    public class StaffDto
    {
        /// <summary>
        /// スタッフID（プライマリキー）
        /// </summary>
        public int StaffId { get; set; }

        /// <summary>
        /// ログインアカウント名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// ログインパスワード
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// スタッフ表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// スタッフ種別
        /// </summary>
        public StaffType StaffType { get; set; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 最終更新者のスタッフID
        /// </summary>
        public int? UpdateStaffId { get; set; }

        /// <summary>
        /// 削除日時
        /// </summary>
        public DateTime? DeleteDate { get; set; }

        /// <summary>
        /// 読み取り専用プロパティ：削除済みかどうか
        /// </summary>
        public bool IsDeleted
        {
            get
            {
                return (this.DeleteDate is DateTime) ? true : false;
            }
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public StaffDto()
        {
            this.StaffId = -1;
            this.Account = string.Empty;
            this.Password = string.Empty;
            this.DisplayName = string.Empty;
            this.StaffType = StaffType.MEMBER;
            this.UpdateDate = null;
            this.UpdateStaffId = null;
            this.DeleteDate = null;
        }
    }
}