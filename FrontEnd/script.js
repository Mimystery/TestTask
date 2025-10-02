const form = document.getElementById("loginForm");
const message = document.getElementById("message");

form.addEventListener("submit", async function (e) {
  e.preventDefault();

  const username = document.getElementById("username").value.trim();
  const password = document.getElementById("password").value.trim();

  try {
    const response = await fetch("https://localhost:7164/api/Api/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ username, password })
    });

    if (!response.ok) {
      throw new Error(`Server error: ${response.status}`);
    }

    const textResponse = await response.text();
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(textResponse, "text/xml");
    const returnNode = xmlDoc.getElementsByTagName("return")[0];
    if (!returnNode) throw new Error("No <return> in response");

    const resultJson = JSON.parse(returnNode.textContent);

    if ("ResultCode" in resultJson) {
        message.innerHTML = `
        <div class="alert alert-danger">
            ${resultJson.ResultMessage}
        </div>
        `;
    } else if ("Status" in resultJson && resultJson.Status === 0) {
        message.innerHTML = `
        <div class="alert alert-success">
            Login successful! <br>
        </div>
        `;
    }

  } catch (error) {
    message.innerHTML = `
      <div class="alert alert-danger">
        Error calling API: ${error}
      </div>
    `;
  }
});
