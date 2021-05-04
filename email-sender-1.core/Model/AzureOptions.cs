namespace email_sender_1.core.Model
{
  public class AzureOptions
  {
    public static string Position = "Azure";
    public string EmailSenderPrimary_BlobStorageConnectionString { get; set; }
    public string ContainerName { get; set; }
  }
}
