using System;
using PlayFab.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayfabFriendsController : MonoBehaviour
{
    private void Start()
    {
        GetFriendsList();
    }

    public void GetFriendsList()
    {
        var request = new GetFriendsListRequest
        {
            IncludeFacebookFriends = false,
            IncludeSteamFriends = false,
            XboxToken = null,
        };
        PlayFabClientAPI.GetFriendsList(request, OnFriendsListUpdated,PlayFabUtils.OnError);
    }

    private void OnFriendsListUpdated(GetFriendsListResult result)
    {
        var friends = result.Friends;
        var txt = $"Friends List [{friends.Count}\n]";
        foreach (var f in friends)
        {
            txt += $"-{f.TitleDisplayName}[{f.FriendPlayFabId}]\n";
        }
        Debug.Log(txt);
    }

    [SerializeField] private string _friendDisplayNameToAdd;
    public void AddFriend() => AddFriendByDisplayName(_friendDisplayNameToAdd);
    public void AddFriendByDisplayName(string displayName)
    {
        var request = new AddFriendRequest {FriendTitleDisplayName = displayName,};
        PlayFabClientAPI.AddFriend(request,OnFriendAdded,PlayFabUtils.OnError);
    }
    public void AddFriendByUserName(string userName)
    {
        var request = new AddFriendRequest {FriendUsername = userName,};
        PlayFabClientAPI.AddFriend(request,OnFriendAdded,PlayFabUtils.OnError);
    }
    public void AddFriendByEmail(string email)
    {
        var request = new AddFriendRequest {FriendEmail = email,};
        PlayFabClientAPI.AddFriend(request,OnFriendAdded,PlayFabUtils.OnError);
    }
    public void AddFriendByPlayfabId(string playfabId)
    {
        var request = new AddFriendRequest {FriendPlayFabId = playfabId,};
        PlayFabClientAPI.AddFriend(request,OnFriendAdded,PlayFabUtils.OnError);
    }
    private void OnFriendAdded(AddFriendResult result)
    {
        Debug.Log($"Friend Added[{result.ToJson()}]");
    }

    [SerializeField] private string _friendIdToRemove;
    public void RemoveFriend() => RemoveFriend(_friendIdToRemove);
    public void RemoveFriend(string playfabId)
    {
        var request = new RemoveFriendRequest {FriendPlayFabId = playfabId,};
        PlayFabClientAPI.RemoveFriend(request,OnFriendRemoved, PlayFabUtils.OnError);
    }

    private void OnFriendRemoved(RemoveFriendResult obj)
    {
        Debug.Log($"Player Removed From FriendList[{obj.ToJson()}]");
    }
}