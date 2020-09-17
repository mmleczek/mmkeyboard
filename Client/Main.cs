using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Client
{
    public class Main:BaseScript
    {
        public bool allowmove = false;
        public bool CanWalk = false;
        public bool IsVisible = false;
        public int CurrentRequestId = 0;
        public bool IsMainTaskWorking = false;


        Dictionary<int, CallbackDelegate> PendingRequests = new Dictionary<int, CallbackDelegate>();

        public Main()
        {
            Exports.Add("Show", new Action<bool, int, CallbackDelegate>((can_walk, maxlenght, cb) => 
            {
                try
                {
                    if (!IsVisible)
                    {
                        if (cb != null)
                        {
                            int req = GetRequestId();
                            IsVisible = true;
                            CanWalk = can_walk;
                            SendNuiMessage("{ \"show\": true }");
                            SendNuiMessage("{ \"request\": " + req.ToString() + " }");
                            SendNuiMessage("{ \"maxlength\": " + maxlenght.ToString() + " }");
                            PendingRequests.Add(req, cb);
                            if(!IsMainTaskWorking)
                            {
                                SetNuiFocus(true, true);
                                SetNuiFocusKeepInput(true);
                                IsMainTaskWorking = true;
                                Tick += OnTick;
                            }
                        }
                        else
                        {
                            Log.Error("Callback is null");
                        }
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(ex);
                }
            }));

            Exports.Add("IsVisible", new Func<bool>(() => { return IsVisible; }));

            Exports.Add("Hide", new Action(() => Hide()));

            RegisterNUICallback("allowmove", NUI_allowmove);
            RegisterNUICallback("response", NUI_response);
        }


        public void Hide()
        {
            SendNuiMessage("{ \"hide\": true }");
            IsVisible = false;
            new Task(async () =>
            {
                GC.Collect();
                await Delay(200);
                SetNuiFocusKeepInput(false);
                SetNuiFocus(false, false);
                IsMainTaskWorking = false;
                Tick -= OnTick;
            }).Start();
        }

        private async Task OnTick()
        {
            try
            {
                DisableAllControlActions(0);
                EnableControlAction(0, 249, true);
                //EnableControlAction(0, 80, true);
                EnableControlAction(0, 166, true);
                EnableControlAction(0, 167, true);
                EnableControlAction(0, 168, true);
                DisableControlAction(0, 138, true);
                DisableControlAction(0, 52, true);

                if (allowmove && CanWalk)
                {
                    EnableControlAction(0, 30, true);
                    EnableControlAction(0, 31, true);
                    EnableControlAction(0, 32, true);
                    EnableControlAction(0, 33, true);
                    EnableControlAction(0, 34, true);
                    EnableControlAction(0, 35, true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            await Task.FromResult(0);
        }

        public int GetRequestId()
        {
            if (CurrentRequestId < 65535)
            {
                CurrentRequestId = CurrentRequestId + 1;
                return CurrentRequestId;
            }
            else
            {
                CurrentRequestId = 0;
                return CurrentRequestId;
            }
        }

        private CallbackDelegate NUI_allowmove(IDictionary<String, object> data, CallbackDelegate result)
        {
            try
            {
                bool tgl = data.GetVal<bool>("allowmove", false);
                allowmove = tgl;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return result;
        }

        private CallbackDelegate NUI_response(IDictionary<String, object> data, CallbackDelegate result)
        {
            try
            {
                int req = data.GetVal<int>("request", -1);
                if (req != -1)
                {
                    string text = data.GetVal<string>("value", string.Empty);
                    
                    if(PendingRequests.ContainsKey(req))
                    {
                        PendingRequests[req].Invoke(text);
                        PendingRequests.Remove(req);
                    }
                }
                Hide();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return result;
        }

        #region NUI Implementation

        private void RegisterNUICallback(string msg, Func<IDictionary<string, object>, CallbackDelegate, CallbackDelegate> callback)
        {
            RegisterNuiCallbackType(msg);

            EventHandlers[$"__cfx_nui:{msg}"] += new Action<ExpandoObject, CallbackDelegate>((body, resultCallback) =>
            {
                CallbackDelegate err = callback.Invoke(body, resultCallback);
            });
        }

        #endregion
    }
    public static class DictionaryExtensions
    {
        public static T GetVal<T>(this IDictionary<string, object> dict, string key, T defaultVal)
        {
            if (dict.ContainsKey(key))
                if (dict[key] is T)
                    return (T)dict[key];
            return defaultVal;
        }
    }
}
