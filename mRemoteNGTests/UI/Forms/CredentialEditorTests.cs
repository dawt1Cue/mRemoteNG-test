﻿using mRemoteNG.Credential;
using mRemoteNG.Security;
using mRemoteNG.UI.Controls;
using mRemoteNG.UI.Forms;
using NUnit.Framework;
using NUnit.Extensions.Forms;

namespace mRemoteNGTests.UI.Forms
{
    [TestFixture]
    public class CredentialEditorTests
    {
        private FrmCredentialEditor _credentialEditorForm;
        private CredentialList _credentialList;

        [SetUp]
        public void Setup()
        {
            _credentialList = new CredentialList {new CredentialInfo()};
        }

        [TearDown]
        public void TearDown()
        {
            _credentialList = null;
            _credentialEditorForm?.Dispose();
        }

        private void CreateCredentialEditorForm(CredentialInfo credentialInfo = null)
        {
            _credentialEditorForm = new FrmCredentialEditor(_credentialList, credentialInfo);
            _credentialEditorForm.Show();
        }

        [Test]
        public void CredentialEditorFormLoadsWhenNotGivenAnExistingCredentialInfo()
        {
            CreateCredentialEditorForm();
            var credentialFormTester = new FormTester(_credentialEditorForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(1));
        }

        [Test]
        public void CredentialEditorFormLoadsWhenGivenAnExistingCredentialInfo()
        {
            CreateCredentialEditorForm(new CredentialInfo());
            var credentialFormTester = new FormTester(_credentialEditorForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(1));
        }

        [Test]
        public void CancelClosesForm()
        {
            CreateCredentialEditorForm();
            var cancelButton = new ButtonTester("btnCancel");
            cancelButton.Click();
            var credentialFormTester = new FormTester(_credentialEditorForm.Name);
            Assert.That(credentialFormTester.Count, Is.EqualTo(0));
        }

        [Test]
        public void ClickingCancelDoesNotAddNewCredentialToList()
        {
            CreateCredentialEditorForm();
            var cancelButton = new ButtonTester("btnCancel");
            cancelButton.Click();
            Assert.That(_credentialList.Count, Is.EqualTo(1));
        }

        [Test]
        public void ClickingCancelButtonDoesNotUpdateExistingCredentialItem()
        {
            var credentialToUpdate = _credentialList[0];
            var originalEntryName = credentialToUpdate.Name;
            var updatedEntryName = "MyUpdatedEntryName";
            CreateCredentialEditorForm(credentialToUpdate);
            var entryNameTextbox = new TextBoxTester("txtEntryName");
            entryNameTextbox.Enter(updatedEntryName);
            var cancelButton = new ButtonTester("btnCancel");
            cancelButton.Click();
            Assert.That(_credentialList[credentialToUpdate].Name, Is.EqualTo(originalEntryName));
        }

        [Test]
        public void CueBannerSetOnPasswordFieldWhenEditingAlreadyExistingPassword()
        {
            var credential = new CredentialInfo {Password = "testPassword".ConvertToSecureString()};
            CreateCredentialEditorForm(credential);
            var passwordBox = new TextBoxTester("secureTextBoxManualEntryPassword");
            var cueBannerText = passwordBox.Properties.GetCueBannerText();
            Assert.That(cueBannerText, Is.Not.Null);
        }

        [Test]
        public void EditingExistingCredentialItemUpdatesCredentialListAfterClickingOk()
        {
            var updatedEntryName = "MyUpdatedEntryName";
            var credentialToUpdate = _credentialList[0];
            CreateCredentialEditorForm(credentialToUpdate);
            var entryNameTextbox = new TextBoxTester("txtEntryName");
            var okButton = new ButtonTester("btnOk");
            entryNameTextbox.Enter(updatedEntryName);
            okButton.Click();
            Assert.That(_credentialList[credentialToUpdate].Name, Is.EqualTo(updatedEntryName));
        }

        [Test]
        public void EditingExistingCredentialItemDoesNotAddANewItemToList()
        {
            var credentialToUpdate = _credentialList[0];
            CreateCredentialEditorForm(credentialToUpdate);
            var okButton = new ButtonTester("btnOk");
            okButton.Click();
            Assert.That(_credentialList.Count, Is.EqualTo(1));
        }

        [Test]
        public void CreatingNewCredentialItemAddsItemToListAfterClickingOk()
        {
            CreateCredentialEditorForm();
            var okButton = new ButtonTester("btnOk");
            okButton.Click();
            Assert.That(_credentialList.Count, Is.EqualTo(2));
        }

        [Test]
        public void EditingUserNameTextBoxUpdatesCredentialObject()
        {
            var credentialToEdit = _credentialList[0];
            CreateCredentialEditorForm(credentialToEdit);
            var userNameTextbox = new TextBoxTester("txtManualEntryUsername");
            var newUserName = "User123";
            userNameTextbox.Enter(newUserName);
            var okButton = new ButtonTester("btnOk");
            okButton.Click();
            Assert.That(_credentialList[credentialToEdit].Username, Is.EqualTo(newUserName));
        }

        [Test]
        public void EditingDomainTextBoxUpdatesCredentialObject()
        {
            var credentialToEdit = _credentialList[0];
            CreateCredentialEditorForm(credentialToEdit);
            var domainTextbox = new TextBoxTester("txtManualEntryDomain");
            var newDomain = "User123";
            domainTextbox.Enter(newDomain);
            var okButton = new ButtonTester("btnOk");
            okButton.Click();
            Assert.That(_credentialList[credentialToEdit].Domain, Is.EqualTo(newDomain));
        }
    }
}