using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class LibraryContext
    {
        private SqlConnectionStringBuilder connect;
        public LibraryContext()
        {
            connect = new SqlConnectionStringBuilder();
            connect.InitialCatalog = "Library";
            connect.DataSource = @"";  // write your server name (if you have login and password, you need add parametrs)
            connect.ConnectTimeout = 30;
            connect.IntegratedSecurity = true;
        }
        public List<Book> getBooks()
        {
            List<Book> booksList = new List<Book>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connect.ConnectionString;
                try
                {
                    conn.Open();
                    DataTable inv = new DataTable();
                    string strSql = "Select * From Books";
                    SqlCommand selectCommand = new SqlCommand(strSql, conn);
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    inv.Load(reader);

                    foreach (DataRow row in inv.Rows)
                    {
                        booksList.Add(new Book()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            NameBook = row["NameBook"].ToString(),
                            Quantity = int.Parse(row["Quantity"].ToString()),
                            YearPublish = int.Parse(row["YearPublish"].ToString())
                        });
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
            return booksList;
        }

        public List<Author> getAuthors()
        {
            List<Author> authorsList = new List<Author>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connect.ConnectionString;
                try
                {
                    conn.Open();
                    DataTable inv = new DataTable();
                    string strSql = "Select * From Author";
                    SqlCommand selectCommand = new SqlCommand(strSql, conn);
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    inv.Load(reader);

                    foreach (DataRow row in inv.Rows)
                    {
                        authorsList.Add(new Author()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString()
                        });
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
            return authorsList;
        }


        public void addBooks(BookRequest book, List<Author> authorList)
        {

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connect.ConnectionString;
                try
                {
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "InsertBookAndAuthors";
                    command.Parameters.AddWithValue("@bookName", book.NameBook);
                    command.Parameters.AddWithValue("@Quantity", book.Quantity);
                    command.Parameters.AddWithValue("@YearPublish", book.YearPublish);
                    command.Parameters.AddWithValue("@bookId", 0);
                    SqlParameter retValue = command.Parameters.Add("@bookId", SqlDbType.Int);
                    retValue.Direction = ParameterDirection.ReturnValue;

                    int result = command.ExecuteNonQuery();
                    int bookId = 0;
                    if (result != 0)
                    {
                        bookId = (int)retValue.Value;
                    }
                    if (bookId != 0 && book.AuthorId != 0)
                    {
                        command = new SqlCommand("insert into BooksAuthors(BookId, AuthorId) VALUES (@bookId, @authorId)", conn);
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.Parameters.AddWithValue("@authorId", book.AuthorId);
                        command.ExecuteNonQuery();
                    }

                    if (bookId != 0 && authorList.Count != 0)
                    {
                        int authorId = 0;
                        foreach (var author in authorList)
                        {
                            command = conn.CreateCommand();
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "InsertAuthor";
                            command.Parameters.AddWithValue("@firstName", author.FirstName);
                            command.Parameters.AddWithValue("@lastName", author.LastName);
                            command.Parameters.AddWithValue("@authorId", 0);
                            retValue = command.Parameters.Add("@authorId", SqlDbType.Int);
                            retValue.Direction = ParameterDirection.ReturnValue;
                            result = command.ExecuteNonQuery();
                            if (result != 0)
                            {
                                authorId = (int)retValue.Value;
                            }
                            if (authorId != 0)
                            {
                                command = new SqlCommand("insert into BooksAuthors(BookId, AuthorId) VALUES (@bookId, @authorId)", conn);
                                command.Parameters.AddWithValue("@bookId", bookId);
                                command.Parameters.AddWithValue("@authorId", authorId);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }


        public void deleteBook(int bookId)
        {
            if (bookId != 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();
                        //string strSql = "delete * from Books where Id = " + bookId;
                        //SqlCommand selectCommand = new SqlCommand(strSql, conn);
                        //selectCommand.ExecuteNonQuery();
                        var command = new SqlCommand("delete from BooksAuthors where BookId = @bookId", conn);
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.ExecuteNonQuery();

                        command = new SqlCommand("delete from Books where Id = @bookId", conn);
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        public Book getBookById(int bookId)
        {
            Book book = new Book();

            if (bookId != 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();

                        DataTable inv = new DataTable();
                        var command = new SqlCommand("select * from Books where Id = @bookId", conn);
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();
                        inv.Load(reader);

                        foreach (DataRow row in inv.Rows)
                        {
                            book = new Book()
                            {
                                Id = int.Parse(row["Id"].ToString()),
                                NameBook = row["NameBook"].ToString(),
                                Quantity = int.Parse(row["Quantity"].ToString()),
                                YearPublish = int.Parse(row["YearPublish"].ToString())
                            };
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return book;
        }

        public List<Author> GetAutherForBookById(int bookId)
        {
            List<Author> authorList = new List<Author>();

            if (bookId != 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();

                        DataTable inv = new DataTable();
                        var command = new SqlCommand("Select Author.Id, FirstName, LastName from Author inner join BooksAuthors on Author.Id = BooksAuthors.AuthorId where BookId = @bookId", conn);
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();
                        inv.Load(reader);

                        foreach (DataRow row in inv.Rows)
                        {
                            authorList.Add(new Author()
                            {
                                Id = int.Parse(row["Id"].ToString()),
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString(),
                            });
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return authorList;
        }


        public void deleteAuthorFromBookById(int authorId)
        {
            if (authorId != 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();

                        var command = new SqlCommand("delete from BooksAuthors where AuthorId = @authorId", conn);
                        command.Parameters.AddWithValue("@authorId", authorId);
                        command.ExecuteNonQuery();

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        public void UpdateBookAndAuthor(Book book, List<Author> authorList)
        {
            if (book != null)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        if (book.Id != 0)
                        {
                            conn.Open();
                            SqlCommand command = new SqlCommand("Update Books Set NameBook = @nameBook, Quantity = @quantity, YearPublish = @yearPublish where Id = @bookId", conn);
                            command.Parameters.AddWithValue("@bookId", book.Id);
                            command.Parameters.AddWithValue("@nameBook", book.NameBook);
                            command.Parameters.AddWithValue("@quantity", book.Quantity);
                            command.Parameters.AddWithValue("@yearPublish", book.YearPublish);
                            command.ExecuteNonQuery();

                            if (authorList.Count != 0)
                            {
                                int authorId = 0;
                                int result = 0;
                                foreach (var author in authorList)
                                {
                                    if (author.Id == 0)
                                    {
                                        command = conn.CreateCommand();
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.CommandText = "InsertAuthor";
                                        command.Parameters.AddWithValue("@firstName", author.FirstName);
                                        command.Parameters.AddWithValue("@lastName", author.LastName);
                                        command.Parameters.AddWithValue("@authorId", 0);
                                        SqlParameter retValue = command.Parameters.Add("@authorId", SqlDbType.Int);
                                        retValue.Direction = ParameterDirection.ReturnValue;
                                        result = command.ExecuteNonQuery();
                                        if (result != 0)
                                        {
                                            authorId = (int)retValue.Value;
                                        }
                                        if (authorId != 0)
                                        {
                                            command = new SqlCommand("insert into BooksAuthors(BookId, AuthorId) VALUES (@bookId, @authorId)", conn);
                                            command.Parameters.AddWithValue("@bookId", book.Id);
                                            command.Parameters.AddWithValue("@authorId", authorId);
                                            command.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        command = new SqlCommand("Update Author Set FirstName = @firstName, LastName = @lastName where Id = @authorId", conn);
                                        command.Parameters.AddWithValue("@authorId", author.Id);
                                        command.Parameters.AddWithValue("@firstName", author.FirstName);
                                        command.Parameters.AddWithValue("@lastName", author.LastName);

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }


        public List<User> getUsers()
        {
            List<User> usersList = new List<User>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connect.ConnectionString;
                try
                {
                    conn.Open();
                    DataTable inv = new DataTable();
                    string strSql = "Select * From Users";
                    SqlCommand selectCommand = new SqlCommand(strSql, conn);
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    inv.Load(reader);

                    foreach (DataRow row in inv.Rows)
                    {
                        usersList.Add(new User()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Mail = row["Email"].ToString(),
                            PhoneNumber = long.Parse(row["PhoneNumber"].ToString()),
                        });
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
            return usersList;
        }


        public int CreateNewUser(User newUser)
        {
            int userId = 0;

            if (newUser != null)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();
                        SqlCommand command = conn.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertUsers";
                        command.Parameters.AddWithValue("@firstName", newUser.FirstName);
                        command.Parameters.AddWithValue("@lastName", newUser.LastName);
                        command.Parameters.AddWithValue("@email", newUser.Mail);
                        command.Parameters.AddWithValue("@phoneNumber", newUser.PhoneNumber);
                        command.Parameters.AddWithValue("@userId", 0);

                        SqlParameter retValue = command.Parameters.Add("@userId", SqlDbType.Int);
                        retValue.Direction = ParameterDirection.ReturnValue;

                        int result = command.ExecuteNonQuery();
                        if (result != 0)
                        {
                            userId = (int)retValue.Value;
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
            return userId;
        }

        public void AddHistory(int bookId, int quantity, History newHistory)
        {
            if (newHistory != null && quantity != 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("insert into History(BookId, UserId, DateOfIssue, DateReturn) values(@bookId, @userId, @dateOfIssue, @dateReturn)", conn);
                        command.Parameters.AddWithValue("@bookId", newHistory.BookId);
                        command.Parameters.AddWithValue("@userId", newHistory.UserId);
                        command.Parameters.AddWithValue("@dateOfIssue", newHistory.DateOfIssue);
                        command.Parameters.AddWithValue("@dateReturn", newHistory.DateReturn);
                        command.ExecuteNonQuery();


                        quantity -= 1;

                        command = new SqlCommand("update Books set Quantity = @quantity where Id = @bookId", conn);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@bookId", bookId);
                        command.ExecuteNonQuery();

                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }

        public List<History> getListHistories()
        {
            List<History> historiesList = new List<History>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connect.ConnectionString;
                try
                {
                    conn.Open();
                    DataTable inv = new DataTable();
                    string strSql = "select History.Id, BookId, UserId, DateOfIssue, DateReturn, Books.NameBook, Users.FirstName, Users.LastName from History inner join Books on BookId = Books.Id inner join Users on UserId = Users.Id";
                    SqlCommand selectCommand = new SqlCommand(strSql, conn);
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    inv.Load(reader);

                    foreach (DataRow row in inv.Rows)
                    {
                        historiesList.Add(new History()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            BookId = int.Parse(row["BookId"].ToString()),
                            UserId = int.Parse(row["UserId"].ToString()),
                            DateOfIssue = DateTime.Parse(row["DateOfIssue"].ToString()),
                            DateReturn = DateTime.Parse(row["DateReturn"].ToString()),
                            BookName = row["NameBook"].ToString(),
                            UserName = row["FirstName"].ToString() + " " + row["LastName"].ToString(),
                        });
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
            return historiesList;
        }

        public User getUserById(int userId)
        {
            User user = new User();

            if (userId != 0)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connect.ConnectionString;
                    try
                    {
                        conn.Open();

                        DataTable inv = new DataTable();
                        var command = new SqlCommand("select * from Users where Id = @userId", conn);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();
                        inv.Load(reader);

                        foreach (DataRow row in inv.Rows)
                        {
                            user = new User()
                            {
                                Id = int.Parse(row["Id"].ToString()),
                                FirstName = row["FirstName"].ToString(),
                                LastName = row["LastName"].ToString(),
                                Mail = row["Email"].ToString(),
                                PhoneNumber = long.Parse(row["PhoneNumber"].ToString()),
                            };
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return user;
        }
    }
}
