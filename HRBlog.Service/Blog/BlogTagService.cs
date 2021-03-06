﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Dapper;
using HRBlog.Model.Blog;
using HRBlog.Service.Sys;

namespace HRBlog.Service.Blog
{
    public class BlogTagService
    {
        public string ConnStr;
        public BlogTagService(string conn)
        {
            ConnStr = conn;
        }
        /// <summary>
        /// 插入标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(T_BLOG_TAG model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_BLOG_TAG(");
            strSql.Append("TagName,CreateUser,CreateTime,UpdateUser,UpdateTime,Remark)");

            strSql.Append(" values (");
            strSql.Append("@TagName,@CreateUser,@CreateTime,@UpdateUser,@UpdateTime,@Remark)");
            strSql.Append(" ;SELECT @@IDENTITY;");
            using (var conn = ConnentionFactory.GetOpenSqlConnection(ConnStr))
            {
                int tagBlogID=conn.Query<int>(strSql.ToString(), model).First();
                return tagBlogID;
            }
        }

        public int Insert(T_BLOG_TAG model,IDbTransaction transaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_BLOG_TAG(");
            strSql.Append("TagName,CreateUser,CreateTime,UpdateUser,UpdateTime,Remark)");

            strSql.Append(" values (");
            strSql.Append("@TagName,@CreateUser,@CreateTime,@UpdateUser,@UpdateTime,@Remark)");
            strSql.Append(" ;SELECT @@IDENTITY;");
            using (var conn = ConnentionFactory.GetOpenSqlConnection(ConnStr))
            {
                int tagBlogID = conn.Query<int>(strSql.ToString(), model, transaction).First();
                return tagBlogID;
            }
        }

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(T_BLOG_TAG model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update T_BLOG_TAG set ");
            strSql.Append("TagName=@TagName,");
            strSql.Append("CreateUser=@CreateUser,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("UpdateUser=@UpdateUser,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where TagID=@TagID");
            using (var conn=ConnentionFactory.GetOpenSqlConnection(ConnStr))
            {
                int resBlogID=conn.Execute(strSql.ToString(), model);
                return resBlogID > 0;
            }
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T_BLOG_TAG> GetList(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TagID,TagName,CreateUser,CreateTime,UpdateUser,UpdateTime,Remark ");
            strSql.Append(" FROM T_BLOG_TAG ");
            if (!string.IsNullOrEmpty(where))
            {
                strSql.Append(" where " + where);
            }
            using (var conn=ConnentionFactory.GetOpenSqlConnection(ConnStr))
            {
               var list= conn.Query<T_BLOG_TAG>(strSql.ToString()).ToList();
                return list;
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            string strSql = "delete from T_BLOG_TAG where TagID=@id";
            using (var conn=ConnentionFactory.GetOpenSqlConnection(ConnStr))
            {
                int resBlogID=conn.Execute(strSql, new {id = id});
                return resBlogID > 0;
            }
        }
    }
}
