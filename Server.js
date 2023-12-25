const express = require('express'); // 用于创建 Web 服务器
const multer = require('multer');   // 处理文件上传
const path = require('path');   // 处理文件路径
const bodyParser = require('body-parser');  // 处理 JSON 请求。
const fs = require('fs');   //  模块进行文件系统操作

const app = express();  // 创建一个 Express 应用
const port = 80;

// Multer 配置了一个磁盘存储引擎。
// destination 函数确定上传文件的存储目录，filename 函数确定上传文件的名称。
const storage = multer.diskStorage({ 
  destination: function (req, file, cb) {
    
    const account = req.body.account; // 从请求体中获取账户信息
    const uploadPath = path.join('uploads', account); // 使用账户名作为子文件夹

    // 检查文件夹是否存在，如果不存在，则创建它
    if (!fs.existsSync(uploadPath)) {
      fs.mkdirSync(uploadPath, { recursive: true });
    }

    // cb是一个回调函数，调用它来通知 Multer 文件已经保存了
    cb(null, uploadPath);   // 将文件保存到指定路径：uploadPath
  },

  // cb是一个回调函数，通知 multer 文件名已经确定。
  filename: function (req, file, cb) {
    cb(null, file.originalname);    // 使用原始文件名作为文件名
  }
});

const upload = multer({ storage: storage });

// 使用 body-parser 处理 JSON 请求
app.use(bodyParser.json());

// 定义了一个 Express 路由来处理发送到 '/upload' 端点的 POST 请求。
// 处理文件上传
// upload.single('file') 中间件用于处理单个文件上传，它期望文件被发送到请求中的名为 'file' 的字段。
app.post('/upload', upload.single('file'), (req, res) => {
  const file = req.file;
  if (!file) {
    return res.status(400).send('No file uploaded.');
  }
  res.send('File uploaded!');
});

// 处理文件下载
app.get('/download/:account/:filename', (req, res) => {
  const account = req.params.account;
  const filename = req.params.filename;
  const filePath = path.join(__dirname, 'uploads', account, filename);
  console.log(account);
  console.log(filename);
  console.log(filePath);
  // 使用 Express 的 sendFile 方法发送文件
  res.sendFile(filePath, (err) => {
    if (err) {
      console.error("Error sending file: ", err.message);
      res.status(err.status).end();
    } else {
      console.log("File sent successfully");
    }
  });
});

// 启动服务器
app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});

这段代码的逻辑是什么样的