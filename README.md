![A synthwave styled logo for VisitorGraph](https://raw.githubusercontent.com/matt-goldman/VisitorGraph/main/assets/synth_logo.png)

[![Hack Together: Microsoft Graph and .NET](https://img.shields.io/badge/Microsoft%20-Hack--Together-orange?style=for-the-badge&logo=microsoft)](https://github.com/microsoft/hack-together)

# A visitor sign-in system using Microsoft Graph, Azure AD, and Microsoft Teams

Created for Microsoft Hack Together (https://github.com/microsoft/hack-together)

Check it out in action in this video:

https://user-images.githubusercontent.com/19944129/222936935-98fa7cb5-c543-4d80-a9ad-d50621ff04fa.mp4

## Set it up in your own tenant

To use this app, create an app registration in Azure AD with the following Microsoft Graph Application permissions:
* Chat.Create
* User.Read.All

Create a client secret for this app registration, and add it along with the other details to Key Vault or your app secrets. You will also need a user account with a Teams license assigned (used to send notification messages). This should be treated as a service account, rather than an account for a user. You will need to add this account's credentials to the app's secrets provider too.