document.addEventListener("DOMContentLoaded", function () {

        //抓聊天小圖示跟放聊天室的地方
        const showChatList = document.getElementById('showChatList');
        const chatContainerPart = document.getElementById('chatContainerPart');

        //展開關閉聊天室div
        showChatList.addEventListener('click', function () {
            console.log("Click event fired");
            if (chatContainerPart.style.display === 'none') {
                chatContainerPart.style.display = 'block';
            } else {
                chatContainerPart.style.display = 'none';
            }
        });

    });

//固定的參數
const getChatUserInfo = document.getElementById('getChatUseInfo');
const senderIdPart = parseInt(getChatUserInfo.getAttribute('data-login-id'), 10); // 抓登入者ID，使用10進位轉換
const userApiUrl = getChatUserInfo.getAttribute('data-userList-api');//抓聊天對象apiURL
const chatDetailApiUrl = getChatUserInfo.getAttribute('data-chatdetail-api');//抓聊天詳細apiURL
const chatLoginAvaApiUrl = getChatUserInfo.getAttribute('data-loginUserAva-api');//抓登入者頭像apiURL
let inChatRoomPart = 0;//先確定使用者是在跟哪個對象聊天
let chatAvaterUrlListPart = [];//存左側聊天對象們頭像用
let chatAvatarPart = document.createElement('img');//左側點到誰存那個頭像網址
let addAvaUrlPart;
let loginUserAvatarUrl;

//抓登入者頭像網址
fetch(chatLoginAvaApiUrl)
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.text(); 
    })
    .then(data => {
        // 在這裡處理從 API 返回的數據（data）
        loginUserAvatarUrl = data;
        console.log(loginUserAvatarUrl);
    })
    .catch(error => {
        // 處理錯誤
        console.error('There was a problem with the fetch operation:', error);
    });




//建立signalR連線
const connectionPart = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/chat") // 填寫Hub端點URL 這邊要跟program.cs寫得一模一樣，要不然不會抓到app.MapHub<ChatHub>("/hubs/chat");
    .build();

connectionPart.start().then(() => {
    const connectionIdPart = connectionPart.connectionId;
    console.log("SignalR Connected. Connection ID: " + connectionIdPart);
    // 呼叫後端的 UpdateUserInfo 方法，傳遞 accountId 和 connectionId
    connectionPart.invoke('UpdateUserInfo', senderIdPart, connectionIdPart);
}).catch(err => { console.error(err); setTimeout(start, 5000); });

connectionPart.on("ReceiveMessage", function (senderIdPart, receiverIdPart, message, messageTimestamp) {
    console.log("這裡是ReceiveMessage，你接收到訊息會在下面印出:");
    console.log("訊息" + message);
    console.log("receiverIdPart" + receiverIdPart);
    console.log("senderIdPart" + senderIdPart);
    console.log(messageTimestamp);
    console.log("房號" + inChatRoomPart);

    // 處理訊息的顯示等操作
    if (inChatRoomPart == senderIdPart) {
        console.log("雖然前面是傳全域的訊息，但現在我要限定囉" + message);
        ImReciver(senderIdPart, receiverIdPart, message, messageTimestamp);
        keepDown();
    }
    loadUser();


});

connectionPart.on("UpdateUserInfo", function (accountId, connectionIdPart) {
    console.log("這裡是UpdateUserInfo，更新使用者清單時會看到我喔");
});

//聊天發出功能--------------------start--------------------
const btnSentMessagePart = document.getElementById("btnSentMessagePart");
const messageInputPart = document.querySelector("input[name='jsSendMessage']");
let receiverIdPart = 0;

btnSentMessagePart.addEventListener('click', () => {//按下畫面上按鈕的事件
    const message = messageInputPart.value; // 取得使用者輸入的訊息
    if (message.trim() !== "") {//如果message移除空白後也不為空的話
        // 呼叫 SendPrivateMessage 方法傳送訊息
        connectionPart.invoke('SendPrivateMessage', parseInt(senderIdPart), parseInt(receiverIdPart), message).catch(err => console.error(err)).then(() => {
            messageInputPart.value = ''; // 清空輸入框
            if (inChatRoomPart !== 0) {
                loadUser();
                console.log("監看傳訊有沒有成功btn");
                const messageTimestamp = Date.now();
                ImSender(senderIdPart, receiverIdPart, message, messageTimestamp);
                keepDown();
            }
        });
    }
})

messageInputPart.addEventListener('keydown', (event) => {//在messageInput按下enter的事件
    if (event.key === 'Enter') { // 如果按下的是 Enter 鍵
        event.preventDefault(); // 防止預設行為（換行）

        const message = messageInputPart.value.trim(); // 取得使用者輸入的訊息，並移除前後空白
        if (message !== "") {
            // 呼叫 SendPrivateMessage 方法傳送訊息
            connectionPart.invoke('SendPrivateMessage', parseInt(senderIdPart), parseInt(receiverIdPart), message).catch(err => console.error(err)).then(() => {
                messageInputPart.value = ''; // 清空輸入框
                if (inChatRoomPart !== 0) {
                    loadUser();
                    console.log("監看傳訊有沒有成功enter");
                    const messageTimestamp = Date.now();
                    ImSender(senderIdPart, receiverIdPart, message, messageTimestamp);
                    keepDown();
                }
            });
        }
    }
});
//聊天發出功能--------------------end--------------------

//即時加入訊息到HTML-----------這裡是接收用
function ImReciver(senderIdPart, receiverIdPart, message, messageTimestamp) {
    const chatDetailPart = document.querySelector('#chatDetailPart');//放到chatDetailPartDiv
    const chatContainerDiv = document.createElement('div');//創一個DIV
    chatContainerDiv.classList.add('chat-container', 'flex', 'flex-row-reverse');//把div加樣式

    const chatMessage = document.createElement('span');//對話
    chatMessage.textContent = message;//對話文字
    chatMessage.classList.add('chat-message');

    const messageTimeEle = document.createElement('span');
    const messageTime = new Date(messageTimestamp);
    const options = { year: '2-digit', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', hour12: false };
    messageTimeEle.textContent = messageTime.toLocaleString(undefined, options);
    messageTimeEle.classList.add('message-time', 'small', 'go-right', 'font-color-grey');

    const brElement = document.createElement('br');

    const addChatAvatar = document.createElement('img');//這個給接收訊息時的對方頭像用
    addChatAvatar.src = addAvaUrlPart;

    chatMessage.classList.add('message-left', 'chat-bubble', 'go-left');//訊息樣式
    addChatAvatar.classList.add('chat-avatar', 'chat-avatar', 'go-left');
    messageTimeEle.classList.add('message-time', 'small', 'go-left');
    chatMessage.appendChild(brElement);
    chatMessage.appendChild(messageTimeEle);//加入對話時間
    chatContainerDiv.appendChild(addChatAvatar);//加入頭像
    chatContainerDiv.appendChild(chatMessage);//加入對話
    console.log(addChatAvatar)

    chatDetailPart.append(chatContainerDiv);
}
//即時加入訊息到HTML-----------接收結束-----------------------------------------

//即時加入訊息到HTML-----------這裡是傳訊用
function ImSender(senderIdPart, receiverIdPart, message, messageTimestamp) {
    const chatDetailPart = document.querySelector('#chatDetailPart');//放到chatDetailDiv
    const chatContainerDiv = document.createElement('div');//創一個DIV
    chatContainerDiv.classList.add('chat-container', 'flex', 'flex-row-reverse');//把div加樣式

    const chatMessage = document.createElement('span');//對話
    chatMessage.textContent = message;//對話文字
    chatMessage.classList.add('chat-message');

    const messageTimeEle = document.createElement('span');
    const messageTime = new Date(messageTimestamp);
    const options = { year: '2-digit', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', hour12: false };
    messageTimeEle.textContent = messageTime.toLocaleString(undefined, options);
    messageTimeEle.classList.add('message-time', 'small', 'go-right', 'font-color-grey');

    const brElement = document.createElement('br');

    const addChatAvatar = document.createElement('img');//這邊是自己的頭像

    addChatAvatar.src = loginUserAvatarUrl;
    addChatAvatar.classList.add('chat-avatar', 'chat-avatar', 'go-right');
    chatMessage.classList.add('message-right', 'chat-bubble', 'go-right');
    messageTimeEle.classList.add('message-time', 'small', 'go-right');
    chatMessage.appendChild(brElement);
    chatMessage.appendChild(messageTimeEle);//加入對話時間
    chatContainerDiv.appendChild(addChatAvatar);//加入頭像
    chatContainerDiv.appendChild(chatMessage);//加入對話

    chatDetailPart.appendChild(chatContainerDiv);
}

//即時加入訊息到HTML-----------傳訊結束-----------------------------------------

//-----------------------------------------------------------------------------------------------------------------------------------------------------

//聊天室預設捲軸在最下方--------------------start--------------------
function keepDown() {
    const chatDetailPart = document.querySelector('#chatDetailPart');
    chatDetailPart.scrollTop = chatDetailPart.scrollHeight;
}

//聊天室預設捲軸在最下方--------------------end--------------------

//這邊是左方聊天對象--------------------start--------------------
const userListPart = document.querySelector('#userListPart');
async function loadUser() {
    chatAvaterUrlListPart = [];

    userListPart.innerHTML = '';
    const response = await fetch(userApiUrl);
    const data = await response.json();
    let chatAvaCount = 0;

    const user = data.map(u => {//抓左邊聊天對象列表的map
        const messageTime = new Date(u.latestMessage.created);//這個是以毫秒為單位
        const nowTime = new Date();
        const timeGap = nowTime - messageTime;
        const messageDate = messageTime.getDate();
        const currentDate = nowTime.getDate();

        let timeDisplay;
        if (currentDate === messageDate) {
            const options = { hour: 'numeric', minute: 'numeric', hour12: false };//24H制
            timeDisplay = messageTime.toLocaleTimeString(undefined, options);
        }
        else if (timeGap < 1 * 24 * 60 * 60 * 1000) //24H*60M*60S 一天內但已經換日
        {
            const options = { hour: 'numeric', minute: 'numeric', hour12: false };
            timeDisplay = '昨天' + messageTime.toLocaleTimeString(undefined, options);
        }
        else if (timeGap < 3 * 24 * 60 * 60 * 1000) //三天內
        {
            const days = Math.floor(timeGap / (24 * 60 * 60 * 1000));
            timeDisplay = `${days} 天前`
        }
        else {//超過三天
            const options = { month: 'numeric', day: 'numeric' };
            timeDisplay = messageTime.toLocaleDateString(undefined, options);
        }


        chatAvaterUrlListPart.push(`data:image/jpeg;base64,${u.memberPhoto}`);//存路徑
        console.log(chatAvaterUrlListPart)

        const chatLinkPart = document.createElement('a');//自動生成的a標籤
        chatLinkPart.href = '#';
        chatLinkPart.classList.add('chatLink', 'link-underline', 'link-underline-opacity-0');
        chatLinkPart.setAttribute('inChatWithIdPart', u.accountId);//與誰聊天
        chatLinkPart.setAttribute('inPage', '1');//設定讀取第幾頁，因為要讀最新的所以都是設定1
        chatLinkPart.setAttribute('data-index', chatAvaCount);//儲存頭像的Index
        const messagePreview = u.latestMessage.message.length > 10 ? `${(u.latestMessage.message).substring(0, 10)}...` : u.latestMessage.message;//預覽最新訊息
        chatLinkPart.innerHTML = //裡面包的html
            `<div class="chat-list-group list-group-flush d-flex align-items-center p-3" style="height:100px" id=${chatAvaCount} onclick="toggleBackgroundColor(this)">
                                 <img src="${chatAvaterUrlListPart[chatAvaCount]}" class="col-2 user-image">
                         <div class="col-10 mx-2">
                                      <span class="">${u.name}</span> <span class="small float-end pe-3">${timeDisplay}</span>
                              </br>
                            <span class="small">${messagePreview}</span>
                           </div>
                 </div>`;
        chatAvaCount++;
        //這邊是左方聊天對象--------------------end--------------------


        //點下聊天對象監聽器(到右邊聊天詳細)--------------------start--------------------
        chatLinkPart.addEventListener('click', async (event) => {
            event.preventDefault();//防止連結預設的#
            const clickedElement = event.currentTarget;//要找到使用者按下了誰
            const index = clickedElement.getAttribute('data-index');//抓取按下的那個index值

            const jsChatWithId = chatLinkPart.getAttribute('inChatWithIdPart')
            receiverIdPart = chatLinkPart.getAttribute('inChatWithIdPart')//這邊都是指對象
            inChatRoomPart = chatLinkPart.getAttribute('inChatWithIdPart')//這邊都是指對象
            console.log(receiverIdPart)

            const jsPage = chatLinkPart.getAttribute('inPage');
            const currentLoginId = senderIdPart;
            const response = await fetch(`${chatDetailApiUrl}?chatWithId=${jsChatWithId}&page=${jsPage}`);

            const chatDetailData = await response.json();

            const chatDetailPart = document.querySelector('#chatDetailPart');
            chatDetailPart.innerHTML = ''; // 清空原內容
            chatDetailData.map(chat => {

                const chatContainerDiv = document.createElement('div');
                chatContainerDiv.classList.add('chat-container', 'flex', 'flex-row-reverse');

                chatAvatarPart = document.createElement('img');
                chatAvatarPart.src = chatAvaterUrlListPart[index];//指定頭像路徑
                addAvaUrlPart = chatAvaterUrlListPart[index];//把網址先存起來

                const chatMessage = document.createElement('span');//對話
                chatMessage.textContent = chat.message;//對話文字
                chatMessage.classList.add('chat-message');

                const messageTimeEle = document.createElement('span');
                const messageTime = new Date(chat.created);
                const options = { year: '2-digit', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', hour12: false };
                messageTimeEle.textContent = messageTime.toLocaleString(undefined, options);
                messageTimeEle.classList.add('message-time', 'small', 'go-right', 'font-color-grey');

                const brElement = document.createElement('br');


                if (chat.receiverId === currentLoginId) {
                    chatMessage.classList.add('message-left', 'chat-bubble', 'go-left');//訊息樣式
                    chatAvatarPart.classList.add('chat-avatar', 'chat-avatar', 'go-left');
                    messageTimeEle.classList.add('message-time', 'small', 'go-left');
                    chatMessage.appendChild(brElement);
                    chatMessage.appendChild(messageTimeEle);//加入對話時間
                    chatContainerDiv.appendChild(chatAvatarPart);//加入頭像
                    chatContainerDiv.appendChild(chatMessage);//加入對話

                } else {
                    chatAvatarPart.src = loginUserAvatarUrl;//這裡是自己的
                    chatAvatarPart.classList.add('chat-avatar', 'chat-avatar', 'go-right');
                    chatMessage.classList.add('message-right', 'chat-bubble', 'go-right');
                    messageTimeEle.classList.add('message-time', 'small', 'go-right');
                    chatMessage.appendChild(brElement);
                    chatMessage.appendChild(messageTimeEle);//加入對話時間
                    chatContainerDiv.appendChild(chatAvatarPart);//加入頭像
                    chatContainerDiv.appendChild(chatMessage);//加入對話
                }
                chatDetailPart.appendChild(chatContainerDiv);
            })
            keepDown();
        })

        userListPart.appendChild(chatLinkPart);

    })
}
//點下聊天對象監聽器(到右邊聊天詳細)--------------------end--------------------

loadUser();

//點選作用中事件
function toggleBackgroundColor(element) {
    element.classList.add('active');
    for (const ta of document.getElementsByClassName('chat-list-group')) {
        if (ta.id !== element.id) {
            ta.classList.remove('active');
        }
    }
}