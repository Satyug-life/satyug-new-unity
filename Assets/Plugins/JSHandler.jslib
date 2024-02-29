mergeInto(LibraryManager.library, {

	RedirectWebSite : function(){
		window.location.href = 'https://activity.digitalrammandir.com/participation';
	},

	Exit : function(atoken,  acollectebleID, auserId){
		window.location.href = `https://activity.digitalrammandir.com/login?token=${UTF8ToString(atoken)}&collectiveId=${UTF8ToString(acollectebleID)}&userId=${UTF8ToString(auserId)}&type=1`;
	},
});