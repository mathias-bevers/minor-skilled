using CommunityToolkit.Mvvm.ComponentModel;
using Encrypt.Library;
using FitMate.Utils;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string password = string.Empty;
    [ObservableProperty]
    private string userName = string.Empty;

    public void OnLoginRequest()
    {
        string encryptedPassword = AESUtil.Encrypt(Password, App.SETTINGS.EncryptionKey);
        SqlCommand select = new("SELECT u.ID FROM Users u WHERE u.UserName = @user_name and u.Password = @password");
        select.Parameters.AddWithValue("@user_name", UserName.ToLower());
        select.Parameters.AddWithValue("@password", encryptedPassword);

        int id = -1;
        Task.Run(() => SqlCommunicator.Select(select, reader => { id = Convert.ToInt32(reader["ID"]); }))
            .WaitAndUnwrapException();
        
        System.Diagnostics.Debug.WriteLine($"{UserName}, {Password} = id {id}");

        Password = UserName = string.Empty;

        if (id < 0)
        {
            throw new PopupException("password or username is wrong");
        }
        
        App.SetUserID(id);
        Shell.Current.GoToAsync("//AllWorkouts");
    }

    public void OnRegisterRequest()
    {
        string encryptedPassword = AESUtil.Encrypt(Password, App.SETTINGS.EncryptionKey);
        // SqlCommand insert = new("INSERT INTO USERS, ")
    }
}