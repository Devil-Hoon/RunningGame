using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardedAD : MonoBehaviour
{
	private RewardedAd rewardedAd;
	public Button continueADBtn;
	public Button continueBtn;

	private void Start()
	{
		string adUnitId;
#if UNITY_ANDROID
		adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
	adUnitId = "unexpected_platform";
#endif
		this.rewardedAd = new RewardedAd(adUnitId);

		// Called when an ad request has successfully loaded.
		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		// Called when an ad request failed to load.
		this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
		// Called when an ad is shown.
		this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
		// Called when an ad request failed to show.
		this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
		// Called when the user should be rewarded for interactiong with the ad.
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		// Called when the ad is closed.
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded ad with the request;
		this.rewardedAd.LoadAd(request);


	}
	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdLoaded event received");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToLoad event received with message: " + args.LoadAdError.GetMessage());
	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToSho event received with message: " + args.AdError.GetMessage());
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		this.CreateAndLoadRewardedAd();
		MonoBehaviour.print(
			"HandleRewardedAdClosed event received for " + args.ToString());
	}

	public void HandleUserEarnedReward(object sneder, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print(
			"HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
		continueBtn.gameObject.SetActive(true);
	}

	public void UserChoseToWatchAd()
	{
		if (this.rewardedAd.IsLoaded())
		{
			this.rewardedAd.Show();
			continueADBtn.interactable = false;
		}
	}

	public void CreateAndLoadRewardedAd()
	{
		string adUnitId;
#if UNITY_ANDROID
		adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
	adUnitId = "unexpected_platform";
#endif
		this.rewardedAd = new RewardedAd(adUnitId);

		// Called when an ad request has successfully loaded.
		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		// Called when the user should be rewarded for interactiong with the ad.
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		// Called when the ad is closed.
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded ad with the request;
		this.rewardedAd.LoadAd(request);
	}
}
