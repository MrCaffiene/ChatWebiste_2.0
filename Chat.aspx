<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="ChatWebsite_2._0.Chat" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Chat Room - Chat App</title>

    <style>
        /* Global Styles */
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
            height: 100vh;
            background-color: #f4f7fc;
            background-image: url('https://source.unsplash.com/1600x900/?chat,technology');
            background-size: cover;
            background-position: center;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        /* Chat Container */
        .chat-container {
            background-color: rgba(255, 255, 255, 0.9);
            padding: 30px;
            width: 100%;
            max-width: 700px;
            border-radius: 15px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
            display: flex;
            flex-direction: column;
            height: 80vh;
        }

        h1 {
            color: #007bff;
            font-size: 2.5em;
            margin-bottom: 20px;
            text-align: center;
        }

        /* Chat messages container */
        .chat-messages {
            flex-grow: 1;
            overflow-y: auto;
            padding: 10px;
            margin-bottom: 20px;
            border-radius: 10px;
            background-color: #f9f9f9;
            box-shadow: inset 0 0 5px rgba(0, 0, 0, 0.1);
        }

        .message {
            display: flex;
            margin: 10px 0;
            padding: 10px;
            border-radius: 10px;
            word-wrap: break-word;
        }

            .message .username {
                font-weight: bold;
                color: #007bff;
                margin-right: 8px;
            }

            .message .message-text {
                color: #333;
                word-wrap: break-word;
            }

            .message .timestamp {
                font-size: 0.8em;
                color: #888;
                margin-left: 10px;
            }

            /* Align messages differently based on sender */
            .message.current-user {
                justify-content: flex-end;
                background-color: #007bff;
                color: white;
            }

            .message.other-user {
                justify-content: flex-start;
                background-color: #f1f1f1;
            }

        /* Message input area */
        .message-input-container {
            display: flex;
            flex-direction: column;
        }

        textarea {
            width: 100%;
            padding: 12px;
            font-size: 16px;
            border: 1px solid #ccc;
            border-radius: 8px;
            resize: none;
            margin-bottom: 10px;
            transition: all 0.3s ease;
        }

            textarea:focus {
                border-color: #007bff;
                box-shadow: 0 0 5px rgba(0, 123, 255, 0.3);
                outline: none;
            }

        button {
            padding: 12px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

            button:hover {
                background-color: #0056b3;
            }

        /* Typing indicator */
        .typing-indicator {
            font-size: 0.9em;
            color: #888;
            margin-top: 10px;
            font-style: italic;
        }

        /* Header with Username & Logout Button */
        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }

            .header .username {
                font-size: 1.2em;
                font-weight: bold;
                color: #007bff;
            }

            .header .logout-btn {
                padding: 8px 16px;
                background-color: #dc3545;
                color: white;
                border: none;
                border-radius: 8px;
                cursor: pointer;
            }

                .header .logout-btn:hover {
                    background-color: #c82333;
                }

        /* Dark mode and light mode toggle */
        .dark-mode .chat-container {
            background-color: #333;
            color: #fff;
        }

        .dark-mode textarea {
            background-color: #444;
            color: white;
            border-color: #666;
        }

        .dark-mode button {
            background-color: #0056b3;
        }

        /* Responsive Design */
        @media (max-width: 480px) {
            .chat-container {
                padding: 20px;
                width: 90%;
            }

            h1 {
                font-size: 2em;
            }

            button {
                font-size: 14px;
            }
        }
    </style>
</head>

<body>

    <form id="form1" runat="server">
        <div class="chat-container">
            <!-- Header with Username and Logout Button -->
            <div class="header">
                <span class="username">
                    <%: Session["Username"] %>
                    <!-- Display logged-in username -->
                </span>
                <button class="logout-btn" type="submit" runat="server" onserverclick="btnLogout_Click">Logout</button>
            </div>

            <h1>Chat Room</h1>

            <!-- Chat messages container -->
            <div class="chat-messages">
                <asp:Repeater ID="rptMessages" runat="server">
                    <ItemTemplate>
                        <div class="message <%# Eval("Username") == Session["Username"] ? "current-user" : "other-user" %>">
                            <span class="username"><%# Eval("Username") %>:</span>
                            <span class="message-text"><%# Eval("MessageText") %></span>
                            <span class="timestamp"><%# Eval("Timestamp", "{0:HH:mm:ss}") %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Typing indicator -->
            <div class="typing-indicator" id="typingIndicator" runat="server">
                <!-- Display typing indicator here dynamically -->
            </div>

            <!-- Message input and send button -->
            <div class="message-input-container">
                <textarea id="txtMessage" runat="server" placeholder="Type a message..." rows="4" required></textarea>
                <button type="submit" runat="server" onserverclick="btnSend_Click" class="btn-send">Send</button>
            </div>

            <!-- Error message display -->
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" CssClass="error-message"></asp:Label>
        </div>
    </form>

</body>
</html>
