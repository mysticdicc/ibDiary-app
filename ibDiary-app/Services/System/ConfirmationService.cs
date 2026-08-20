using System;
using System.Collections.Generic;
using System.Text;
using ibDiary_app.Models.Generic;

namespace ibDiary_app.Services.System
{
    public class ConfirmationService
    {
        private Func<ConfirmationRequest, Task>? _showCallback;

        public void RegisterShowCallback(Func<ConfirmationRequest, Task> callback)
        {
            _showCallback = callback;
        }

        public async Task<ConfirmationRequest> ShowAsync(ConfirmationRequest request)
        {
            if (_showCallback == null)
                throw new InvalidOperationException("ConfirmationPopup component not initialized");

            await _showCallback(request);
            return request;
        }
    }
}
