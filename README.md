# AprioritWebCalendar - How to build

Hello, here is instruction how to run the application.

1. Download all repository into your machine.
2. First of all. There must be a file with secrets for JWT. It's ignored in this repository, so you have to create it individually:

  - path: src\AprioritWebCalendar\AprioritWebCalendar.Web\configs\jwtOptions.json
  - <b>Content:</b> here is json. Don't forget to configure "Audience" - url, where it runs.
  <pre>
  {
    "JwtOptions": {
      "Issuer": "WebCalendar",
      "Audience": "http://localhost:65067/",
      "Key": "my_supersecret_key.Nobody can hack the system. Fuck you!!!",
      "Lifetime": 1440
    }
  }
  </pre>
  
  -path: src\AprioritWebCalendar\AprioritWebCalendar.Web\configs\smtpOptions.json - credentials of email account
  
  <pre>
   {
    "SmtpOptions": {
      "Server": "smtp.server.com",
      "Port": 465,
      "UseSsl": true,

      "Login": "",
      "Password": "",

      "FromTitle": "WEB Calendar Project"
    }
  }

  </pre>
  
  3. Client app <b>CalendarModule</b> is in folder "src\AprioritWebCalendar\ClientApps\CalendarModule".
  4. You have to build app or run it via <b>ng serve</b> using proxy-config.
  5. If you are running it using proxy-config, there are 2 files with them in that folder, open one of them and <b>configure port</b>, where back-end part runs.
  
  P.S. I'm not sure that directory for build output is configured well, so make sure too...
