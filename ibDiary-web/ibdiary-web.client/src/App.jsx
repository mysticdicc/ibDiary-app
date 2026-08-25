import { useMemo, useState } from "react";
import "./App.css";

const screenshots = [
    { src: "/images/home.png", alt: "Home screen", title: "Dashboard", description: "Summary of your current medicine, symptoms and pending reports." },
    { src: "/images/history.png", alt: "History Screen", title: "History", description: "View your combined symptom, medicine and food history over time." },
    { src: "/images/history_filter.png", alt: "History Filter", title: "History Filter", description: "Filter your history to see specific details." },
    { src: "/images/foods.png", alt: "Foods screen", title: "Log Foods", description: "Quickly log meals to build a history of how your food choices effect your symptoms." },
    { src: "/images/symptoms.png", alt: "Symptoms screen", title: "Track Symptoms", description: "Build and report on symptoms and their severity over time." },
    { src: "/images/medicines.png", alt: "Medicine Screen", title: "Track Medicines", description: "Build and report on medicines and their schedules over time." },
    { src: "/images/medicine_editor.png", alt: "Medication form screen", title: "Manage Medicines", description: "Build your medicines and log when you take them to get customised notifications." },
    { src: "/images/settings.png", alt: "Settings", title: "Settings", description: "Customise your notification settings and schedule custom reminders."}
];

const initialForm = {
    name: "",
    email: "",
    platform: "Android",
    notes: "",
};

export default function App() {
    const [form, setForm] = useState(initialForm);
    const [status, setStatus] = useState({ type: "idle", message: "" });
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [currentScreenshot, setCurrentScreenshot] = useState(0);

    const nextScreenshot = () => {
        setCurrentScreenshot((prev) => (prev + 1) % screenshots.length);
    };

    const prevScreenshot = () => {
        setCurrentScreenshot((prev) => (prev - 1 + screenshots.length) % screenshots.length);
    };

    const apiBaseUrl = useMemo(() => {
        return import.meta.env.VITE_API_BASE_URL || "";
    }, []);

    const onChange = (e) => {
        const { name, value } = e.target;
        setForm((curr) => ({ ...curr, [name]: value }));
    };

    const validate = () => {
        if (!form.name.trim()) return "Please enter your name.";
        if (!form.email.trim()) return "Please enter your email.";
        const emailOk = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.email);
        if (!emailOk) return "Please enter a valid email address.";
        return "";
    };

    const onSubmit = async (e) => {
        e.preventDefault();

        const error = validate();
        if (error) {
            setStatus({ type: "error", message: error });
            return;
        }

        if (!apiBaseUrl) {
            setStatus({
                type: "error",
                message:
                    "Missing API base URL. Set VITE_API_BASE_URL in your .env file.",
            });
            return;
        }

        setIsSubmitting(true);
        setStatus({ type: "idle", message: "" });

        try {
            // Expected .NET endpoint: POST {apiBaseUrl}/api/alpha-signups
            const res = await fetch(`${apiBaseUrl}/api/alphasignups`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    name: form.name.trim(),
                    email: form.email.trim(),
                    notes: form.notes.trim(),
                    source: "marketing-site",
                }),
            });

            if (!res.ok) {
                const maybeJson = await res.json().catch(() => null);
                const message =
                    maybeJson?.message ||
                    maybeJson?.title ||
                    `Signup failed (${res.status}).`;
                throw new Error(message);
            }

            setStatus({
                type: "success",
                message: "Thanks! You’re on the alpha waitlist 🎉",
            });
            setForm(initialForm);
        } catch (err) {
            setStatus({
                type: "error",
                message:
                    err?.message ||
                    "Something went wrong while submitting your signup.",
            });
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="page">
            <header className="hero container">
                <img src="/images/home.png"/>
                <div className="badge">
                    <h1>IbDiary</h1>
                </div>
                <h1>Track foods, symptoms, and medicines in one place.</h1>
                <p>
                    IbDiary helps people with IBD and related conditions log daily habits,
                    identify triggers, and track history for better care conversations.
                </p>
                <a className="cta" href="#alpha-signup">
                    Join the Alpha
                </a>
            </header>

            <section className="container section">
                <h2>Built around real daily tracking</h2>
                <div className="features">
                    <article className="card">
                        <h3>Foods & Meals</h3>
                        <p>Log foods quickly to generate a history of what you have eaten.</p>
                    </article>
                    <article className="card">
                        <h3>Medicines</h3>
                        <p>
                            Keep active medications, dose schedules, and prescription context
                            in one timeline.
                        </p>
                    </article>
                    <article className="card">
                        <h3>Symptoms</h3>
                        <p>
                            Track symptom patterns over time and connect them to food and
                            medication events.
                        </p>
                    </article>
                </div>
            </section>

            <section className="container section">
                <h2>Tailored for you</h2>
                <div className="custom-section">
                    <article className="card">
                        <h3>Custom Notifications</h3>
                        <p>Enable or disable any notification to suit your preferences, whether you want to be
                        reminded every day or never at all.</p>
                    </article>
                    <article className="card">
                        <h3>100% Local</h3>
                        <p>No cloud features, no data mining. All data is kept locally on the device and can
                        be deleted from the settings at any time.</p>
                    </article>
                </div>
            </section>

            <section className="container section">
                <h2>App Preview</h2>
                <div className="carousel">
                    <button className="carousel-btn carousel-prev" onClick={prevScreenshot} aria-label="Previous screenshot">
                        ‹
                    </button>
                    <div className="carousel-content">
                        <figure className="shot-card">
                            <img src={screenshots[currentScreenshot].src} alt={screenshots[currentScreenshot].alt} />
                        </figure>
                        <div className="shot-info">
                            <h3>{screenshots[currentScreenshot].title}</h3>
                            <p>{screenshots[currentScreenshot].description}</p>
                        </div>
                    </div>
                    <button className="carousel-btn carousel-next" onClick={nextScreenshot} aria-label="Next screenshot">
                        ›
                    </button>
                </div>
                <div className="carousel-dots">
                    {screenshots.map((_, index) => (
                        <button
                            key={index}
                            className={`dot ${index === currentScreenshot ? "active" : ""}`}
                            onClick={() => setCurrentScreenshot(index)}
                            aria-label={`Go to screenshot ${index + 1}`}
                        />
                    ))}
                </div>
            </section>

            <section id="alpha-signup" className="container section">
                <h2>Join the IbDiary Alpha</h2>
                <p className="section-intro">
                    Sign up for early access and help shape features.
                </p>

                <form className="signup-form" onSubmit={onSubmit} noValidate>
                    <label>
                        Name
                        <input
                            type="text"
                            name="name"
                            value={form.name}
                            onChange={onChange}
                            placeholder="Your name"
                            autoComplete="name"
                        />
                    </label>

                    <label>
                        Email
                        <input
                            type="email"
                            name="email"
                            value={form.email}
                            onChange={onChange}
                            placeholder="you@example.com"
                            autoComplete="email"
                        />
                    </label>

                    <label>
                        Notes (optional)
                        <textarea
                            name="notes"
                            value={form.notes}
                            onChange={onChange}
                            placeholder="Anything you'd like us to know?"
                            rows={4}
                        />
                    </label>

                    <button type="submit" disabled={isSubmitting}>
                        {isSubmitting ? "Submitting..." : "Request Alpha Access"}
                    </button>

                    {status.type !== "idle" && (
                        <p
                            className={
                                status.type === "success" ? "form-msg success" : "form-msg error"
                            }
                            role="status"
                        >
                            {status.message}
                        </p>
                    )}
                </form>
            </section>

            <footer className="footer">
                <div className="container footer-inner">
                    <span>© {new Date().getFullYear()} IbDiary</span>
                    <a
                        href="https://github.com/mysticdicc/ibDiary-app"
                        target="_blank"
                        rel="noreferrer"
                    >
                        GitHub
                    </a>
                </div>
            </footer>
        </div>
    );
}