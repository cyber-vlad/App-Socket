# Client-Server Socket Application

Aplicatie dezvoltata in ASP.NET Core, in Visual Studio.

## Descriere

Aplicatie destinata pentru comunicarea full-duplex intre client si server prin socket (protocolul TCP/IP).
Full-duplex comunicarea inseamna ca ambele parti (client si server) pot trimite si primi date simultan.

## Pasii de implementare

1. In ASP.NET Core cream Blank Solution in care vor fi adaugate AppConsole pentru Server si pentru Client.
2. Atat pentru Server, cat si pentru Client se creaza socket, necesar pentru trimiterea si primirea datelor.
3. Pe partea server, prin Bind legam socket-ul cu portul, apoi prin Listen are loc ascultarea pe acel canal conexiunea clientului. Clientul trebuie sa se conecteze la IP adresa si portul serverului.
4. Dupa finisarea schimbului de mesaje, atat serverul, cat si clientul trebuie sa inchida conexiunea prin Close().

## Compilarea si pornirea aplicatiei

1. Deschidem proiectul AppSocket.sln
2. In Solution Explorer, selectam Server, click dreapta pe el, si selectam Set as Startup Project, apoi rulam prin CTRL+F5
3. Repetam pasul 2, doar ca pentru Client. Proiectul Client se poate rulat de mai multe ori pentru a avea mai multi clienti conectati la server.
4. In terminalul clientului scriem un mesaj de salutare, iar serverul afiseaza mesajul in propriul terminal, si retransmite mesajul celorlalti clienti.
5. Pentru finisarea conexiunii, clientul scrie in terminal "exit" si conexiunea se intrerupe.
