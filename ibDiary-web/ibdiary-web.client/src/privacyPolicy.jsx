import "./App.css";

function PrivacyPolicy() {
    return (
        <div className="page privacy-page">
            <div className="badge">
                <h1>Privacy Policy</h1>
            </div>

            <p>The IbDiary app is 100% local, no data collection happens to external servers and you can opt to delete your
                data at any time by uninstalling the app or using the "delete database" button in the settings.</p>
            <p>The IbDiary app will request permissiosn to send notifications to your device, these notifications are used
                remind you when it is time to take your medicine. Opting out of the notifications will not effect your experience
                in any other way, and they can be disabled in the settings at any time even if you opt in.</p>
        </div>
    );
}

export default PrivacyPolicy;